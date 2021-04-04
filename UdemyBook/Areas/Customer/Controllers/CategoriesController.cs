using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models.ViewModels;

namespace UdemyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CategoriesController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CategoriesVM CategoriesVM { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCategory(string categoryName)
        {
            CategoriesVM = new CategoriesVM
            {
                CategorizedProducts = _unitOfWork.Product.GetAll(book => book.Category.Name == categoryName, includeProperties: "Category,Author")
            };
            return View(CategoriesVM);
        }

    }
}
