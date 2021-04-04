using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UdemyBook.DataAccess.Data;
using UdemyBook.DataAccess.Repositories.IRepository;
using UdemyBook.Models;

namespace UdemyBook.DataAccess.Repositories
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderHeader OrderHeader)
        {
            _db.Update(OrderHeader);
        }
    }
}
