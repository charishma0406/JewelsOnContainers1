using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.ViewModel;

namespace WebMVC.ViewModels
{
    //we are binding this catalogindexviewmodel to our web controller
    //we are creating this for displaying the brands, types and pagination info, and all the catalog items 
    public class CatalogIndexViewModel
    {
        //for page size,page index, previous, next
        public PaginationInfo PaginationInfo { get; set; }

        //selectlistitem is for displaying the dropdowns in the view page
        public IEnumerable<SelectListItem> Brands { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        //whole data
        public IEnumerable<CatalogItem> CatalogItems { get; set; }

        //we are creating int? beacuse it is a null type in the int parameter
        public int? BrandFilterApplied { get; set; }
        public int? TypesFilterApplied { get; set; }

    }
}
