using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models;
using UdemyBook.Models.ViewModels;
using UdemyBook.Ultility;

namespace UdemyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public DetailVM DetailVM { get; set; } = new DetailVM();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int? id)
        {
            if (DetailVM == null)
            {
                DetailVM = new DetailVM();
            }

            DetailVM.UsedToBuyList = _unitOfWork.Product
                                        .GetAll(us => us.Category.Name == SD.Development || us.Category.Name == SD.Design, includeProperties: "Author,Category");
            DetailVM.Cart = new ShoppingCart();
            DetailVM.Cart.Product = _unitOfWork.Product
                                .GetFirstOrDefault(x => x.ID == id, includeProperties: "Author,Category");
            DetailVM.Cart.ProductId = DetailVM.Cart.Product.ID;
            DetailVM.Cart.Price = DetailVM.Cart.Product.Price;
            return View(DetailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Detail()
        {
            ShoppingCart cart = DetailVM.Cart;
            cart.Id = 0;
            //Them6 vao2 cart
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.AppUserId = claim.Value;

            ShoppingCart cartFromDb =
                _unitOfWork.ShoppingCart.GetFirstOrDefault(
                    c => c.AppUserId == cart.AppUserId && c.ProductId == cart.ProductId,
                    includeProperties: "Product"
                    );

            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(cart);
            }
            else
            {
                cartFromDb.Count += cart.Count;
            }
            _unitOfWork.Save();

            var count =
                _unitOfWork.ShoppingCart.GetAll(c => c.AppUserId == cart.AppUserId)
                                        .ToList().Count();
            HttpContext.Session.SetInt32(SD.ShoppingCartSs, count);
            return RedirectToAction($"ConfirmAddToCart");
        }

        public IActionResult ConfirmAddToCart(int id)
        {
            return View();
        }
    }
}
