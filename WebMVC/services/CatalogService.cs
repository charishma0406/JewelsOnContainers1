using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.ViewModel;

namespace WebMVC.services
{
    //creating catalog class and implementing icatalogservice
    public class CatalogService : ICatalogService
    {
        //we are expecting configur to read the external url and giving the base url
        private readonly string _baseUri;
        private readonly IHttpClient _client;



        //we are injecting and asking give me the configuration
        //so who is gng to pass the configuration is by injection by the start up.
        //we are expecting configur to read the external url that given below
        public CatalogService(IConfiguration config, IHttpClient client)
        {
            //read from the config, base uri is coming from startup file
            _baseUri = $"{config["CatalogUrl"]}/api/catalog";
            _client = client;
        }
        //plugging our services to api paths,from that to http client and all from here
           public async Task<Catalog> GetCatalogItemsAsync(int page, int size)
        {
            //first calling the api paths and in that api paths we are calling catalog and we askng give me the uri for get catalog items and we need to give base uri also
            //base url is coming from configuration as a env var from docker conatiner.
            //we are passing base uri to the catalog items, api path will give back the uri
            //this is just a uri it will tell you where we need to go
            var catalogItemsUri = ApiPaths.Catalog.GetAllCatalogItems(_baseUri, page, size);

            //giving the uri to the http client method(getstringasync) and sending back the data in a string
            var dataString = await _client.GetStringAsync(catalogItemsUri);

            //so here it is coming back in string format but we need to deserailaze and sent to controller so we are using jsonconvert.deserialize
            //take datastring and desrlize to catalog and give it  back to me that is the response.
            return JsonConvert.DeserializeObject<Catalog>(dataString);
            



        }
    }
}
