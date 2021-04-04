using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyBook.DataAccess.Repositories.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IAuthorRepository Author { get; }

        IShoppingCartRepository ShoppingCart { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IApplicationUserRepository User { get; }



        ISP_Call SP_Call { get; }

        void Save();
    }
}
