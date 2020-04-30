using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.services;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    //controller is making 3 service calls, getcatalogitems, getbrands,get types
    public class CatalogController : Controller
    {
        //how to call services from here through dependency injection.(we dont want controller to talk drctly to my services so dong DI in start up)

        private readonly ICatalogService _service;
        public CatalogController(ICatalogService service)
        {
            _service = service;
        }
        //the client will tell what page it is. so view will tell that
        //int? is if the page number is null
        public async Task<IActionResult> Index(int? page)
        {

            //telling how many pages we need to show on the ui.
            var itemsOnPage = 10;
            //calling the method in the catalog service 
            //page?? 0 is a terinary operator if page is nul then 0. 
            var catalog = await _service.GetCatalogItemsAsync(page ?? 0, itemsOnPage);
            //there is a class called catalogindexviewmodel from there we are passing the data here
            var vm = new CatalogIndexViewModel
            {
                CatalogItems = catalog.Data,
                PaginationInfo = new PaginationInfo
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = itemsOnPage,
                    TotalItems = catalog.Count,
                    //total pages are 15/10(total 15 count divided by itemson page)
                    //ceiling meaning rounded to the next number. 
                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsOnPage)
                },
                //calling brands method from service
                Brands = await _service.GetBrandsAsync(),
                //calling types method from types
                Types = await _service.GetTypesAsync(),
            };

            
            return View(vm);
        }
    }
}
        


    


        