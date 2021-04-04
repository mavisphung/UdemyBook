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
    public class AuthorsController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AuthorsController(IUnitOfWork unit, IWebHostEnvironment hostEnvironment)
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
            Author author = new Author();
            if (id == null)
            {
                return View(author);
            }
            //author = _unit.author.GetFirstOrDefault(cate => cate.ID == id);
            author = _unit.Author.Get(id.GetValueOrDefault());
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Author author)
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

                    if (author.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webRootPath, author.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filestreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestreams);
                    }
                    author.ImageUrl = @"\images\products\" + fileName + extension;
                }
                else
                {
                    if (author.ID != 0)
                    {
                        Author authorFromDb = _unit.Author.Get(author.ID);
                        authorFromDb.ImageUrl = author.ImageUrl;
                    }
                }


                if (author.ID == 0)
                {
                    _unit.Author.Add(author);
                }
                else
                {
                    _unit.Author.Update(author);
                }
                _unit.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObjects = _unit.Author.GetAll();
            return Json(new { data = allObjects });
        }

        [HttpDelete]
        public IActionResult Delete(int iD)
        {
            var objFromDb = _unit.Author.GetFirstOrDefault(o => o.ID == iD);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //xóa link ảnh nằm trong database và xóa cả ảnh
            string root = _hostEnvironment.WebRootPath;
            if (objFromDb.ImageUrl != null) //tức là object này có chứa ảnh
            {
                string imagePath = Path.Combine(root, objFromDb.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _unit.Author.Remove(objFromDb);
            _unit.Save();
            return Json(new { success = true, message = "Delete successfully" });
        }
        #endregion
    }
}
