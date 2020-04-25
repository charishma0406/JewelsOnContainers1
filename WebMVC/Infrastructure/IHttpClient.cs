using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMVC.Infrastructure
{
    //that is the one that defines how a communication happn btw service and the microservice world.
    //each services calling hhtp client
    //we need to write all the http methods here because every service uses this client. 
    public interface IHttpClient
    {
        //for get request we are returning back a json file in string format so that is y the return type is string for get reuest. Because we are not sending any body to backend.
        //we want multithreaded way so that is y we are using Task because we don't want user to wait.(we have wrap the  return in the task)
        //this method needs to know where is the end point is.Already apipath told that where the endpoint is located. So, now http client know where the endpoint is located.
        Task<string> GetStringAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer");


        //post request it like creating the new data so that is y it needs body to create the data in the backend. 
        //So that is the reason we are using httpresponsemessage(it will send back the response as the status is good(200), status is bad like that). I f we send the data it will send the response back it is good or not.
        //to be used as multiple services like catalog, cart service, order service. So we are using generics here<T>
        // we can make methods as generics.
        Task<HttpResponseMessage> PostAsync<T>(string uri, T items, string authorizationToken = null, string authorizationMethod = "Bearer");


        //put is to update the data
        Task<HttpResponseMessage> PutAsync<T>(string uri, T items, string authorizationToken = null, string authorizationMethod = "Bearer");


        //delete is to delete the data. No need generics here because we no need to give it as other data. we can give it as url itself.
        Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer");
        





    }
}
