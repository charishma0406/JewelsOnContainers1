using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.ViewModel
{
    //catalog is equal to the paginates items in the product catalog microservice
    //replicating the view model in the product catalog project and putting it here to send the http response back in the paginated view type
    public class Catalog
    {
        //creating prop for the page size how many pages
        public int PageSize { get; set; }

        //creating prop for the page index which page we are right now
        public int PageIndex { get; set; }

        //creating the property for which page we are in to know
        public long Count { get; set; }

        //we dont want to be generic because wat we are expecting to see is list of catalog items
        //we are replicating the catalog items in the product catalog service in web side
        public List<CatalogItem> Data { get; set; }


    }
}
