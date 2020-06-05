using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.CartModels;

namespace WebMVC.services
{
    public class CartService : ICartService
    {

        private readonly IConfiguration _config;
        private IHttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private IHttpContextAccessor _httpContextAccesor;
        private readonly ILogger _logger;

        //we are injeceting 3 thing. Ihttpcontext accessor is the to retrive the token from the identity server
        public CartService(IConfiguration config, IHttpContextAccessor httpContextAccesor,
            IHttpClient httpClient, ILoggerFactory logger)
        {
            _config = config;
            //this is the cart accessor
            _remoteServiceBaseUrl = $"{_config["CartUrl"]}/api/cart";
            _httpContextAccesor = httpContextAccesor;
            _apiClient = httpClient;
            _logger = logger.CreateLogger<CartService>();
        }

        //adding an item to the cart
        public async  Task AddItemToCart(ApplicationUser user, CartItem product)
        {
            //get cart method is downbelow
            var cart = await GetCart(user);
            _logger.LogDebug("User Name: " + user.Email);

            //if user want to increase the same quantity then 
            //create cart already have an item
            var basketItem = cart.Items
                .Where(p => p.ProductId == product.ProductId)
                .FirstOrDefault();
            //if the basket is null
            if (basketItem == null)
            {
                //add product
                cart.Items.Add(product);
            }
            else
            {
                //if not increase the quantity
                basketItem.Quantity += 1;
            }

            //update the cart
            await UpdateCart(cart);
        }

        public async Task ClearCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            var cleanBasketUri = ApiPaths.Basket.CleanBasket(_remoteServiceBaseUrl, user.Email);
            _logger.LogDebug("Clean Basket uri : " + cleanBasketUri);
            var response = await _apiClient.DeleteAsync(cleanBasketUri);
            _logger.LogDebug("Basket cleaned");
        }

        //giving the userr information as a application user
        public async Task<Cart> GetCart(ApplicationUser user)
        {
            //creating a method for this below getusertokenasync
            var token = await GetUserTokenAsync();
            _logger.LogInformation(" We are in get basket and user id " + user.Email);
            _logger.LogInformation(_remoteServiceBaseUrl);

            //api path call to the basket
            //get the url and the user id
            //user.email is our email the id of the person
            var getBasketUri = ApiPaths.Basket.GetBasket(_remoteServiceBaseUrl, user.Email);
            _logger.LogInformation(getBasketUri);
            //this is the call to custom http client given the token and uri
            var dataString = await _apiClient.GetStringAsync(getBasketUri, token);
            _logger.LogInformation(dataString);

            //deserializing it into the cart, if the string is empty and creating a new cart for us
            var response = JsonConvert.DeserializeObject<Cart>(dataString.ToString()) ??
                //new cart
               new Cart()
               {
                   //this is our email id 
                   BuyerId = user.Email
               };
            //sending the response back
            return response;
        }

       /* public Order MapCartToOrder(Cart cart)
        {
            var order = new Order();
            order.OrderTotal = 0;

            cart.Items.ForEach(x =>
            {
                order.OrderItems.Add(new OrderItem()
                {
                    ProductId = int.Parse(x.ProductId),

                    PictureUrl = x.PictureUrl,
                    ProductName = x.ProductName,
                    Units = x.Quantity,
                    UnitPrice = x.UnitPrice
                });
                order.OrderTotal += (x.Quantity * x.UnitPrice);
            });

            return order;
        }*/

        public async  Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities)
        {
            var basket = await GetCart(user);

            basket.Items.ForEach(x =>
            {
                // Simplify this logic by using the
                // new out variable initializer.
                if (quantities.TryGetValue(x.Id, out var quantity))
                {
                    x.Quantity = quantity;
                }
            });

            return basket;
        }

        //updating the cart
        public async Task<Cart> UpdateCart(Cart cart)
        {
            //it gets the token first
            var token = await GetUserTokenAsync();
            _logger.LogDebug("Service url: " + _remoteServiceBaseUrl);

            //gets the apipaths to update and give the url
            var updateBasketUri = ApiPaths.Basket.UpdateBasket(_remoteServiceBaseUrl);
            _logger.LogDebug("Update Basket url: " + updateBasketUri);
            //httpclient call
            var response = await _apiClient.PostAsync(updateBasketUri, cart, token);
            //give the response back ensuresuccessstatus code is if the status is good then give ok otherwise it is going to be failed
            response.EnsureSuccessStatusCode();

            return cart;
        }

        //
        async Task<string> GetUserTokenAsync()
        {
            //get the current browser we arre on and give me the current context(header or user information or like that)
            var context = _httpContextAccesor.HttpContext;
            //gettokenasync is inbuilt method asking which type of toekn do you need
            return await context.GetTokenAsync("access_token");

        }

    }
}
