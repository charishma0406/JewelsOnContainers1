using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public async Task<Catalog> GetCatalogItemsAsync(int page, int size, int? brand, int? type)
        {
            //first calling the api paths and in that api paths we are calling catalog and we askng give me the uri for get catalog items and we need to give base uri also
            //base url is coming from configuration as a env var from docker conatiner.
            //we are passing base uri to the catalog items, api path will give back the uri
            //this is just a uri it will tell you where we need to go
            var catalogItemsUri = ApiPaths.Catalog.GetAllCatalogItems(_baseUri, page, size, brand, type);

            //giving the uri to the http client method(getstringasync) and sending back the data in a string
            var dataString = await _client.GetStringAsync(catalogItemsUri);

            //so here it is coming back in string format but we need to deserailaze and sent to controller so we are using jsonconvert.deserialize
            //take datastring and desrlize to catalog and give it  back to me that is the response.
            return JsonConvert.DeserializeObject<Catalog>(dataString);
            



        }

        //plugging our microservice to apipath and getting all types

        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            //this is my uri to get catalogtype
           var typeUri =  ApiPaths.Catalog.GetAllTypes(_baseUri);

            //we need to make httpclient call.
            //this will give return me in the json format
           var dataString =  await _client.GetStringAsync(typeUri);
            //to make as a dropown we are making this

            //we are making an emptylist in the dropdown
            var items = new List<SelectListItem>
           { 
                //we are adding one item
              new SelectListItem
              {
                  //whenever user comes to page by derfault we are showing all the types and brandss on the page
                  
                 //the value is null
                 Value = null,
                 //this is what user can see the text on the dropdown
                 Text = "All",
                 
                 Selected = true

              }

           };

            //we are using jarray newtons lib to deserilaze the json string format into id and type in my dropdown we are parsing.
            //JArray parses to string into a collection
           var types =  JArray.Parse(dataString);

            // we are adding each item, in my each item reading the id value as string and giving it as value and text 
            foreach(var type in types)
            {
                items.Add(new SelectListItem
                {
                    //reading the values of id and give it to me as a string(schema of types is id and type)
                    Value = type.Value<string>("id"),
                    Text = type.Value<string>("type"),


                }
                );
            }
            //returning the items back this is now going back to the controller
            return items;

        }


        //from here it will go to web side catalog controller to give the brands and type, we will call this method in controller.
        /*   public async Task<IEnumerable<SelectListItem>> GetBrandsAsync()
           {
              var brandUri=  ApiPaths.Catalog.GetAllBrands(_baseUri);
               var dataString = await _client.GetStringAsync(brandUri);
               var items = new List<SelectListItem>
               {
                   new SelectListItem
                   {
                       Value = null,
                       Text = "All",
                       Selected = true
                   }

               };

               var brands = JArray.Parse(dataString);

               foreach(var brand in brands)
               {
                   items.Add(new SelectListItem
                   {
                       Value = brand.Value<string>("id"),
                       Text = brand.Value<string>("brand")
                   }
                   ); 
               }
               return items;

           }*/


        public async Task<IEnumerable<SelectListItem>> GetBrandsAsync()
        {
            var brandUri = ApiPaths.Catalog.GetAllBrands(_baseUri);
            var dataString = await _client.GetStringAsync(brandUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value=null,
                    Text="All",
                    Selected = true
                }
            };
            var brands = JArray.Parse(dataString);
            foreach (var brand in brands)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = brand.Value<string>("id"),
                        Text = brand.Value<string>("brand")
                    }
                );
            }

            return items;
        }
    }
}
