using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.services;

namespace WebMVC.Controllers
{
    public class CatalogController : Controller
    {
        //how to call services from here through dependency injection.(we dont want controller to talk drctly to my services so dong DI in start up)

          private readonly ICatalogService _service;
          public CatalogController(ICatalogService service)
          {
            _service = service;
          }
        //the client will tell what page it is. so view will tell that
        public async Task<IActionResult> Index(int page)
        {

            //telling how many pages we need to show on the ui.
            var itemsOnPage = 10;
            //calling the method in the catalog service and calling the data back in the catalog type
           var catalog=  await _service.GetCatalogItemsAsync(page, itemsOnPage);
            
            //give it to my view/page
            return View(catalog);
        }
    }
}