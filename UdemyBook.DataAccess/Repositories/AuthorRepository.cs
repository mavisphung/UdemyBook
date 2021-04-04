using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UdemyBook.DataAccess.Data;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models;

namespace UdemyBook.DataAccess.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly ApplicationDbContext _db;

        public AuthorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Author author)
        {
            var objFromDb = _db.Authors.FirstOrDefault(obj => obj.ID == author.ID);
            if (objFromDb != null)
            {
                if (author.ImageUrl != null)
                {
                    objFromDb.ImageUrl = author.ImageUrl;
                }
                objFromDb.Name = author.Name;
                objFromDb.PhoneNumber = author.PhoneNumber;
                objFromDb.Address = author.Address;
                objFromDb.Description = author.Description;
                objFromDb.Email = author.Email;
            }
        }
    }
}
