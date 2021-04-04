
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UdemyBook.DataAccess.Data;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models;
using UdemyBook.Ultility;

namespace UdemyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Upsert(int? id)
        //{
        //    Category category = new Category();
        //    if (id == null)
        //    {
        //        return View(category);
        //    }
        //    //category = _unit.Category.GetFirstOrDefault(cate => cate.ID == id);
        //    category = _unit.Category.Get(id.GetValueOrDefault());
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(category);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(ApplicationUser category)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        if (category.ID == 0)
        //        {
        //            _unit.Category.Add(category);
        //        }
        //        else
        //        {
        //            _unit.Category.Update(category);
        //        }
        //        _unit.Save();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(category);
        //}

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.AppUsers.ToList();
            var userRole = _db.UserRoles.ToList();
            var roleList = _db.Roles.ToList();

            foreach (var user in userList)
            {
                string roleId = userRole.FirstOrDefault(ur => ur.UserId == user.Id).RoleId;
                user.Role = roleList.FirstOrDefault(rl => rl.Id == roleId).Name;
            }
            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.AppUsers.FirstOrDefault(user => user.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while locking/unlocking" });
            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation is succeed" });
        }
        #endregion
    }
}
