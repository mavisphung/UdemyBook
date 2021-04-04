using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UdemyBook.DataAccess.Data;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models;

namespace UdemyBook.DataAccess.Repositories
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderDetails OrderDetails)
        {

            _db.Update(OrderDetails);
        }
    }
}
