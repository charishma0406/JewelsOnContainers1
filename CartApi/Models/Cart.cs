using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Models
{
    public class Cart
    {
       //when they login we need to know the buyer id 
        public string BuyerId { get; set; }
        //there will be 0 or more items in our cart so we are using list of cart items
        public List<CartItem> Items { get; set; }


        //creating a constructor
        //cart id is a buyer id
        //when user did not add any item to cart one time also but the cart should be there for the specific buyer with buyer id
        //at that time if there user did not add anyproduct then we are creating new empty cart for that buyer
        public Cart(string cartId)
        {
            BuyerId = cartId;
            Items = new List<CartItem>();
        }
    }
}
