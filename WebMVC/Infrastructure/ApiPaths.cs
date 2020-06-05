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

        //for basket  we are making this class (cart)
        public static class Basket
        { 
            //all these methods should match cart in the services
            //get basket
           public static  string GetBasket(string  baseUri, string basketId)
            {
                //the baseuri and the id of the basket to get the basket
                //baseuri is api/cart/get ation in the cart controller services side
                return $"{baseUri}/{basketId}";
            }

            //update basket
            public static string UpdateBasket(string baseUri)
            {
                //
                return baseUri;
            }

            //delete basket
            public static string CleanBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }
        }

       
        
            public static class Order
            {
                public static string GetOrder(string baseUri, string orderId)
                {
                    return $"{baseUri}/{orderId}";
                }

                //public static string GetOrdersByUser(string baseUri, string userName)
                //{
                //    return $"{baseUri}/userOrders?userName={userName}";
                //}
                public static string GetOrders(string baseUri)
                {
                    return baseUri;
                }
                public static string AddNewOrder(string baseUri)
                {
                    return $"{baseUri}/new";
                }
            }
        }


    }

