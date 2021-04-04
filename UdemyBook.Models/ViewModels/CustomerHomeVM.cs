using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyBook.Models.ViewModels
{
    public class CustomerHomeVM
    {
        public IEnumerable<Product> SuggestList { get; set; }

        public IEnumerable<Category> CategoriesList { get; set; }
        public Dictionary<string, IEnumerable<Product>> HashTable { get; set; }

        public string StrSearch { get; set; }
    }
}
