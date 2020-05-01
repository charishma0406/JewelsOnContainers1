using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.ViewModel;

namespace WebMVC.services
{
    //for extensible in future we aare using interface 
    //with out breaking the connection to contrllr to services if we want to change any implementation.
    //all the calls that need to go to product catalog microservice
    
     public interface ICatalogService
     {
        //when we make a call to the getcatalogitemsasync that will go to http client and get data from microservces and 
        //get back the url back and the string needs to deserialize and send it back to user so that is y we are using catalog in the task
        Task<Catalog> GetCatalogItemsAsync(int page, int size, int? brand, int? type);

        //catalog can also make 2 other calls types and brands
        //selectlistitem is return types(for dropdown we use this return type to convert this into dropdown)
        //Ienumerable is forward only and readonly collection
        Task<IEnumerable<SelectListItem>> GetBrandsAsync();

        Task<IEnumerable<SelectListItem>> GetTypesAsync();


    }
}
