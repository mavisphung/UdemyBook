using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unit;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unit)
        {
            _logger = logger;
            _unit = unit;
            CustomerHomeVM = new CustomerHomeVM
            {
                HashTable = new Dictionary<string, IEnumerable<Product>>(),
                //SuggestList = new List<Product>(),
                //CategoriesList = new List<Category>()
            };
        }

        [BindProperty]
        public CustomerHomeVM CustomerHomeVM { get; set; }

        public IActionResult Index()
        {
            //đưa danh sách sản phẩm và category về trang chủ
            IEnumerable<Product> productsList = _unit.Product.GetAll(includeProperties: "Author,Category");
            IEnumerable<Category> categories = _unit.Category.GetAll();
            if (CustomerHomeVM == null)
            {
                CustomerHomeVM = new CustomerHomeVM
                {
                    HashTable = new Dictionary<string, IEnumerable<Product>>()
                };
            }
            foreach (Category category in categories)
            {
                CustomerHomeVM.HashTable.Add(category.Name, productsList
                                        .Where(product => product.Category.Name == category.Name));
            }
            CustomerHomeVM.SuggestList = productsList.Where(book => book.Category.Name == SD.Development);
            CustomerHomeVM.CategoriesList = categories;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var count = _unit.ShoppingCart
                                .GetAll(c => c.AppUserId == claim.Value)
                                .ToList().Count();
                HttpContext.Session.SetInt32(SD.ShoppingCartSs, count);
            }

            return View(CustomerHomeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search(string search)
        {
            if (CustomerHomeVM == null)
            {
                return RedirectToAction(nameof(Index));
            }
            CustomerHomeVM.SuggestList = _unit.Product
                .GetAll(x => x.Title.Contains(search), includeProperties:"Author,Category");
            CustomerHomeVM.StrSearch = search;
            return View(CustomerHomeVM);
        }

        //public IActionResult Categorize(int id)
        //{
        //    if (CustomerHomeVM == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    CustomerHomeVM.SuggestList = _unit.Product
        //        .GetAll(x => x.Category.ID == id, includeProperties: "Author,Category");
        //    CustomerHomeVM.StrSearch = CustomerHomeVM.SuggestList.FirstOrDefault().Category.Name;
        //    return View(CustomerHomeVM);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
