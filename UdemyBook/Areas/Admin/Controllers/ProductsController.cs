using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models;
using UdemyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using UdemyBook.Ultility;
using Microsoft.AspNetCore.Authorization;

namespace UdemyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(IUnitOfWork unit, IWebHostEnvironment hostEnvironment)
        {
            _unit = unit;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM() {
                Product = new Product(),
                CategoriesList = _unit.Category.GetAll().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ID.ToString()
                }),
                AuthorsList = _unit.Author.GetAll().Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ID.ToString()
                })
            };
            if (id == null)
            {
                return View(productVM);
            }
            //category = _unit.Category.GetFirstOrDefault(cate => cate.ID == id);
            productVM.Product = _unit.Product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
            {
                return NotFound();
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extension = Path.GetExtension(files[0].FileName);
                    
                    if (productVM.Product.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filestreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extension; 
                }
                else
                {
                    if (productVM.Product.ID != 0)
                    {
                        Product productFromDb = _unit.Product.Get(productVM.Product.ID);
                        productVM.Product.ImageUrl = productFromDb.ImageUrl;
                    }
                }

                if (productVM.Product.ID == 0)
                {
                    _unit.Product.Add(productVM.Product);
                }
                else
                {
                    _unit.Product.Update(productVM.Product);
                }
                _unit.Save();
                return RedirectToAction(nameof(Index));
            } 
            else
            {
                productVM.CategoriesList =
                    _unit.Category.GetAll()
                                  .Select(item => new SelectListItem
                                  {
                                      Text = item.Name,
                                      Value = item.ID.ToString()
                                  });
                productVM.AuthorsList =
                    _unit.Author.GetAll()
                                  .Select(item => new SelectListItem
                                  {
                                      Text = item.Name,
                                      Value = item.ID.ToString()
                                  });
                if (productVM.Product.ID != 0)
                {
                    productVM.Product = _unit.Product.Get(productVM.Product.ID);
                }
            }
            return View(productVM);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObjects = _unit.Product.GetAll(includeProperties: "Author,Category");
            return Json(new { data = allObjects });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unit.Product.GetFirstOrDefault(o => o.ID == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //xóa link ảnh nằm trong database
            string root = _hostEnvironment.WebRootPath;
            if (objFromDb.ImageUrl != null) //tức là object này có chứa ảnh
            {
                string imagePath = Path.Combine(root, objFromDb.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _unit.Product.Remove(objFromDb);
            _unit.Save();
            return Json(new { success = true, message = "Delete successfully" });
        }
        #endregion
    }
}
