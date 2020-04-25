using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductCatalogApi.Data;
using ProductCatalogApi.Domain;
using ProductCatalogApi.ViewModel;

namespace ProductCatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        //to need access for the catalog context we are adding this
        public readonly CatalogContext _context;
        //to need access for the configuration we are adding this, startup already know to get the iconfiguration for to to access the appjson config file
        public readonly IConfiguration _config;

        //creating constructor and injecting the catalogcontext data and for changing the url in the catalog seed we are configuring the base url in the app.json file and configuring that here 
        public CatalogController(CatalogContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        [Route ("[action]")]
        //whenever we see return type for async and await we need to put task 
        public async Task<IActionResult> Items([FromQuery]int pageindex = 0, [FromQuery]int pagesize = 6)
        {
            //creating var for the count of the pages and longcountasync means it will count till long
            var itemCount = await _context.CatalogItems.LongCountAsync();


            //getting all the catalog items  and skiping and taking how many records we need
            var items = await _context.CatalogItems.Skip(pageindex * pagesize).Take(pagesize).ToListAsync();



            items = ChangePictureUrl(items);

            //creating object for the paginatedviewmodel class
            var model = new PaginatedItemsViewModel<CatalogItem>
            {
                PageSize = pagesize,
                PageIndex = pageindex,
                Count = itemCount,
                Data = items
            };

            //controller is connected to view model and returning the model back in that
            //model pg siz, pg ind,count of pg and data will be available
            return Ok(model);
        }

        //for changing the pictuire url we are creating this method
        private List<CatalogItem> ChangePictureUrl(List<CatalogItem> items)
        {
            //replacing the picture url with the local http url and why we are saving that in the picture url variable because string is immutable,
            //that means string will not change if we replace also so we are saving in picture url variable itself
            items.ForEach(c => c.PictureUrl = c.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced", _config["ExternalBaseUrl"]));
            return (items);
        }
    }
}