using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CartApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;
        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }


        //hich users you want to read from
        [HttpGet("{id}")]
       
        //if the status is ok then it will give back the cart type back other wise null or like that it will give
       [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> Get(string id )
        {
            //we are calling the redis cache repositry and getting the id if user wants to see the cart items
         var basket =   await _repository.GetCartAsync(id);
            //returning the data basket data back
            return Ok(basket);
        }

        //for updating the cart
        //post
        [HttpPost]
       
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        //user wants to update the cart data. from body means it contains the body
        public async Task<IActionResult> Post([FromBody]Cart value)
        {
            //askng rediscache repository to update the cart data
            var basket = await _repository.UpdateCartAsync(value);
            return Ok(basket);
        }

        [HttpDelete("{id}")]
        //deleting the cart data
        public async void Delete(string id)
        {
            await _repository.DeleteCartAsync(id);
        }
    }
}