using System;
using System.Collections.Generic;
using System.Text;
using UdemyBook.Models;

namespace UdemyBook.DataAccess.Repositories.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetails);
    }
}
