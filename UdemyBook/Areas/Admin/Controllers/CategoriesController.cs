
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models;
using UdemyBook.Ultility;

namespace UdemyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CategoriesController(IUnitOfWork unit, IWebHostEnvironment hostEnvironment)
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
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }
            //category = _unit.Category.GetFirstOrDefault(cate => cate.ID == id);
            category = _unit.Category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\categories");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (category.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webRootPath, category.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filestreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestreams);
                    }
                    category.ImageUrl = @"\images\categories\" + fileName + extension;
                }
                else
                {
                    if (category.ID != 0)
                    {
                        Category categoryFromDb = _unit.Category.Get(category.ID);
                        category.ImageUrl = categoryFromDb.ImageUrl;
                    }
                }

                if (category.ID == 0)
                {
                    _unit.Category.Add(category);
                }
                else
                {
                    _unit.Category.Update(category);
                }
                _unit.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObjects = _unit.Category.GetAll();
            return Json(new { data = allObjects });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unit.Category.GetFirstOrDefault(o => o.ID == id);
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

            _unit.Category.Remove(objFromDb);
            _unit.Save();
            return Json(new { success = true, message = "Delete successfully" });
        }
        #endregion
    }
}
