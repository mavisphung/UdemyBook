using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyBook.Models.ViewModels
{
    public class CategoriesVM
    {
        public Category Category { get; set; }
        public IEnumerable<Product> CategorizedProducts { get; set; }
    }
}
