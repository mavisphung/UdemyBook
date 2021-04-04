using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyBook.Ultility
{
    public static class SD
    {
        public const string Development = "Development";
        public const string Design = "Design";
        public const string Business = "Business";
        public const string Lifestyles = "Lifestyles";

        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";


        public const string ShoppingCartSs = "ShoppingCartSession";

        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
