using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyBook.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategoriesList { get; set; }
        public IEnumerable<SelectListItem> AuthorsList { get; set; }
    }
}
