using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Models
{
    public class CartItem
    {
        /* //product Id
         public string Id { get; set; }
         public string ProductId { get; set; }
         public string ProductName { get; set; }
         public string UnitPrice { get; set; }
         //old unit price is like when we add any item to cart after so many days if we see if the
         //amount cahnges it will show the new price
         public string OldUnitPrice { get; set; }
         public string Quantity { get; set; }
         public string PictureUrl { get; set; }*/


        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }


    }
}
