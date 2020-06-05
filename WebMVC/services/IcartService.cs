
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Models.CartModels;

namespace WebMVC.services
{
   public  interface ICartService
    {
        //get teh cart to see the information. ApplicationUSer is like passing the user information
        Task<Cart> GetCart(ApplicationUser user);
        //adding item to the cart
        Task AddItemToCart(ApplicationUser user, CartItem product);
        //updating the item into the cart
        Task<Cart> UpdateCart(Cart Cart);
        //increasing the quantity in theh cart
        Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities);
        //deleting the cart
        Task ClearCart(ApplicationUser user);
    }
}
