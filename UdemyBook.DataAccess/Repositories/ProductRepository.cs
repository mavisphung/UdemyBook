using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UdemyBook.DataAccess.Data;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models;

namespace UdemyBook.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product product)
        {
            var objFromDb = _db.Products.FirstOrDefault(obj => obj.ID == product.ID);
            if (objFromDb != null)
            {
                if (product.ImageUrl != null)
                {
                    objFromDb.ImageUrl = product.ImageUrl;
                }
                objFromDb.Title = product.Title;
                objFromDb.Description = product.Description;
                objFromDb.Price = product.Price;
                objFromDb.AuthorID = product.AuthorID;
                objFromDb.CategoryID = product.CategoryID;
            }
        }
    }
}
