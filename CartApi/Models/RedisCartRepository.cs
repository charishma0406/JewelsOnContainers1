using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CartApi.Models
{
    public class RedisCartRepository : ICartRepository
    {
        //connection multiplexer is like connecting to redis(location of our redis) like our dbcontext in our sql
        //it is like redistributable it can actual multiple the connection
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        //creating constructor to inject in the startup
       public RedisCartRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            //redis will give the storage to store our data getdatabase is like inbuilt redis method to store the data
            _database = _redis.GetDatabase();
        }

         public async Task<Cart> GetCartAsync(string cartId)
        {
            //asking database to give me the data as a string (data is the buyer id)
            var data = await _database.StringGetAsync(cartId);

            //if the data is null or empty
            if (data.IsNullOrEmpty)
            {
                //that means there is no cache found with that id
                return null;
            }
            //we need to store the entire cart that means we need to deserialize as a string and stored in memory
            //if it finds the data it will give me whole cart as a string so we are deserializing to cart type
            return JsonConvert.DeserializeObject<Cart>(data);
        }

       


        //we can see the what is all in the data base
        public IEnumerable<string> GetUsers()
        {
            //it will locate the nearest server to you
            var server = GetServer();
            //it will go and get all the keys 
            var data = server.Keys();
            //we can write below step like this also
            //return data == null ? null : data.Select(k => k.ToString());
            //it just shorter form of terinary operator if data is select give null or else move forward
            return data?.Select(k => k.ToString());
        }

        private IServer GetServer()
        {
            //getting all the endpoints where the data center are located and it will give array of endpoints
            var endpoint = _redis.GetEndPoints();
            //it will pickup the nearest endpoint and give the data and return back

            return _redis.GetServer(endpoint.First());
        }

        //whenever user added any item to cart update cart will get called and update in the database
        public async Task<Cart> UpdateCartAsync(Cart basket)
        {
            //basket have buyerid and then the list of cart items and serialize into string because we are updating the cart
            var created = await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket));

            //if something is failing
            if(!created)
            {
                return null;
            }

            //after updating my cart and again calling getcartasync method to get the data afterwards
            return await GetCartAsync(basket.BuyerId);
        }
        public async  Task<bool> DeleteCartAsync(string id)
        {
            //deleting the buyer id
            return await _database.KeyDeleteAsync(id);
        }

        
    }
}
