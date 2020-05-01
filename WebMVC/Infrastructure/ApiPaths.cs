using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Infrastructure
{
    //apipath job is to get the endpoint, where would i go and get the data
    //api path job is the give the endpoint where it is located(that is our catalog items) and go fire now
    public class ApiPaths
    {
        //we are writing class in the class because for here we are creating all the end points for multiple ones in the future.
        public static class Catalog
        {
            //service will ask api path can you tell me the path(the end point) that get all the catalog items.
            //goal is to return all the catalog items in one url that is y we are creating a method here

            //we are giving the base uri because it should know the which local host and which port it is using and that port it will take
            //we are giving the page size(page) , page index(take)how many pages to take, which type , which brand to take.
            public static string GetAllCatalogItems(string baseUri,
               int page, int take, int?brand, int?type)
            {
                //putting the string as empty first and then we will add the query in this string
                var filterQs = string.Empty;

                if(brand.HasValue || type.HasValue)
                {
                   // using terinary operator here if else
                   //filtering if brands hasvalue give me brand value as string else null
                    var brandQs = (brand.HasValue) ? brand.Value.ToString() : "null";
                    //filtering if types hasvalue give me type value as string else null
                    var typeQS = (type.HasValue) ? type.Value.ToString() : "null";
                    //giving it to the empty string after filtering typeqs and brandqs
                    filterQs = $"/type/{typeQS}/brand/{brandQs}";
                }
                //if user did not give any brand or type then filterquery will be empty
                
                return $"{baseUri}/items{filterQs}?pageIndex={page}&pageSize={take}";
            }


            //In Catalog service there are 2 more api's so we need to use it here for client. Api path is to know my uri to get to my microservices

            public static string GetAllTypes(string baseUri)
            {
                //baseuri is domain/api/controllername
                return $"{baseUri}/catalogtypes";
            }

            //In Catalog service there are 2 more api's so we need to use it here for client. Api path is to know my uri to get to my microservices
            public static string GetAllBrands(string baseUri)
            {
                //baseuri is domain/api/controllername
                return $"{baseUri}/catalogbrands";
            }

        }
       
    }
}
