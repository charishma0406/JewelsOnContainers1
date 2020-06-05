using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            //if my token is not null then
            if(authorizationToken!= null)
            {
                // in the headers section go to the keyy 
                //called authorization to that add new authorization method value
                requestmessage = new HttpRequestMessage(HttpMethod.Get, uri);
            }

            //how we need to send the data this is where think when are clicking on send button and we are giving req mess
            //here is the message and go send it for me
            //this is the response getting back
           var response = await _client.SendAsync(requestmessage);

          
            //we are not deserialising here because the return type is string
            //read all the content as string and we are returning  back
           return await response.Content.ReadAsStringAsync();
        }


        //creating a new method for post
        //incase if there is no put or post then we are throwing an exception as value must be like in the below
        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            // a new StringContent must be created for each retry 
            // as it is disposed after each call
            //
            var requestMessage = new HttpRequestMessage(method, uri);
            //serialising the object into json string
            Console.WriteLine(JsonConvert.SerializeObject(item));
            //content of a req message now it has a body
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
            //if my authorization token is not null
            if (authorizationToken != null)
            {
                //then fire the call
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }
            
            var response = await _client.SendAsync(requestMessage);

            // raise exception if HttpResponseCode 500 
            // needed for circuit breaker to track fails

            //if there are internal server error or like that
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }

            return response;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T items, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return await DoPostPutAsync(HttpMethod.Post, uri, items, authorizationToken, authorizationMethod);
        }

        public Task<HttpResponseMessage> PutAsync<T>(string uri, T items, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return DoPostPutAsync(HttpMethod.Put, uri, items, authorizationToken, authorizationMethod);
        }

        public Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            throw new NotImplementedException();
        }
    }
}
