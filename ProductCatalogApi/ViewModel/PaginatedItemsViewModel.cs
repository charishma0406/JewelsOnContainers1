using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.ViewModel
{
    //creating view model for the ui binding 
    //making this class as generics because we can use this anywhere instead of hard coding the data 
    //like catalog items, carts and orders for evrything we should use all these
    //class is for reference type because our data is reference type we are using class 
    public class PaginatedItemsViewModel<TEntity> where TEntity: class
    {
        //creating prop for the page size how many pages
        public int PageSize { get; set; }

        //creating prop for the page index which page we are right now
        public int PageIndex { get; set; }

        //creating the property for which page we are in to know
        public long Count { get; set; }

        //making this as generic and giving name as data because we can use this anywhere
        public IEnumerable<TEntity> Data { get; set; }


    }
}
