using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UdemyBook.DataAccess.Data;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models;

namespace UdemyBook.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category category)
        {
            var objFromDb = _db.Categories.FirstOrDefault(obj => obj.ID == category.ID);
            if (objFromDb != null)
            {
                objFromDb.Name = category.Name;
                if (category.ImageUrl != null)
                {
                    objFromDb.ImageUrl = category.ImageUrl;
                }
            }
        }
    }
}
