using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMVC.Infrastructure
{
    public class CustomHttpClient : IHttpClient
    {
        //http client is component/protocol/class in the .netcore.
        //why we are not injecting because we need http client everywhere that is y we are not using dependcy inje here
        //http client is the inbuilt class in the .netcore. This is going to make the actual call to our api.
        private readonly HttpClient _client;
        //creating custom and instatiating new http client here
        public CustomHttpClient()
        {
            //everytime fresh http call should run
            _client = new HttpClient();
        }

        //we need to know the request going back so that why we are using asyn and await(it should wait till the req is comingback)
        public async  Task<string> GetStringAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            //we are requesting the message from frontside so we are using
            //httprequest message and getting data and send to uri where my data is contain
            var requestmessage = new HttpRequestMessage(HttpMethod.Get, uri);
            //if you pass me a token what we need to do
            if(authorizationToken!= null)
            {

            }

            //how we need to send the data this is where think when are clicking on send button and we are giving req mess
            //here is the message and go send it for me
            //this is the response getting back
           var response = await _client.SendAsync(requestmessage);

          
            //we are not deserialising here because the return type is string
            //read all the content as string and we are returning  back
           return await response.Content.ReadAsStringAsync();
        }

        public Task<HttpResponseMessage> PostAsync<T>(string uri, T items, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> PutAsync<T>(string uri, T items, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            throw new NotImplementedException();
        }
    }
}
