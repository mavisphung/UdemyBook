using System;
using System.Collections.Generic;
using System.Text;
using UdemyBook.Models;

namespace UdemyBook.DataAccess.Repositories.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
