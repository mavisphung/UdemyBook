using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UdemyBook.DataAccess.Data;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models;
using UdemyBook.Ultility;

namespace UdemyBook.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            public string Address { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string City { get; set; }

            public string Password { get; set; }
            public string ConfirmPassword { get; set; }

            public string Role { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var userFromDb = _unitOfWork.User.GetFirstOrDefault(u => u.Id == user.Id);
            
            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                City = userFromDb.City,
                Address = userFromDb.Address,
                Name = userFromDb.Name,
                Role = userFromDb.Role
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //var user = await _userManager.GetUserAsync(User);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.User.GetFirstOrDefault(u => u.Id == claim.Value);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (user.Role == null)
            {
                await _userManager.AddToRoleAsync(user, SD.Role_Customer);
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.Address != user.Address)
            {
                user.Address = Input.Address;
            }

            if (Input.City != user.City)
            {
                user.City = Input.City;
            }
            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }
            if (Input.Password != null)
            {
                if (Input.Password == Input.ConfirmPassword)
                {
                    if (user.PasswordHash == null)
                    {
                        await _userManager.AddPasswordAsync(user, Input.Password);
                    }
                    else
                    {
                        await _userManager.ChangePasswordAsync(user, user.PasswordHash, Input.Password);
                    }
                }
                else
                {
                    StatusMessage = "Password and confirm password are not matched!";
                    return RedirectToPage();
                }
            }
            _unitOfWork.User.Update(user);
            _unitOfWork.Save();
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
