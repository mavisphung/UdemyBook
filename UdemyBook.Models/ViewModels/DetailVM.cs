using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyBook.Models.ViewModels
{
    public class DetailVM
    {
        public IEnumerable<Product> UsedToBuyList { get; set; }
        public ShoppingCart Cart { get; set; }
    }
}
