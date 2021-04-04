using System;
using System.Collections.Generic;
using System.Text;
using UdemyBook.Models;

namespace UdemyBook.DataAccess.Repositories.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser appUser);
    }
}
