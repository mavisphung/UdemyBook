using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models.ViewModels;
using UdemyBook.Models;
using System.Security.Claims;
using UdemyBook.Ultility;
using Microsoft.AspNetCore.Http;

namespace UdemyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart
                                      .GetAll(u => u.AppUserId == claim.Value, includeProperties: "Product")
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.AppUser =
                _unitOfWork.User.GetFirstOrDefault(x => x.Id == claim.Value);
            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = list.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Product.Price * list.Count);
                list.Product.Description = SD.ConvertToRawHtml(list.Product.Description);
                list.Product.Author = _unitOfWork.Author.GetFirstOrDefault(auth => auth.ID == list.Product.AuthorID);
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "...";
                }
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Product");
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Product");
            if (cart.Count == 1)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(u => u.AppUserId == cart.AppUserId).Count();
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.ShoppingCartSs, count - 1);
            }
            else
            {
                cart.Count -= 1;
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Product");
            var count = _unitOfWork.ShoppingCart.GetAll(u => u.AppUserId == cart.AppUserId).Count();
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.ShoppingCartSs, count - 1);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CheckOut(string userId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = _unitOfWork.ShoppingCart.GetAll(c => c.AppUserId == claim.Value);
            _unitOfWork.ShoppingCart.RemoveRange(cartList);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.ShoppingCartSs, 0);
            return View();
        }
    }
}
