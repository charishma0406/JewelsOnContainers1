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
        [Route("[action]")]
        //whenever we see return type for async and await we need to put task 
        public async Task<IActionResult> Items([FromQuery]int pageindex = 0, [FromQuery]int pagesize = 6)
        {
            //creating var for the count of the pages and longcountasync means it will count till long
            //getting the catalog items from our catalogcontext
            var itemCount = await _context.CatalogItems.LongCountAsync();


            //getting all the catalog items  and skiping and taking how many records we need
            //orderby which allows us to filter by alphabetical order by name
            var items = await _context.CatalogItems
                .OrderBy( c =>c.Name)
                .Skip(pageindex * pagesize)
                .Take(pagesize)
                .ToListAsync();
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

        //writing anotherapi for filtering out the brands and types seperately
        [HttpGet]
        [Route("[action]/type/{catalogTypeId}/brand/{catalogBrandId}")]

        public async Task<IActionResult> Items(int? catalogTypeId, int? catalogBrandId, [FromQuery]int pageIndex = 0, [FromQuery] int pageSize = 6)
        {
            //we are filtering this using Iquerable. it is like filtering types and brands from the catalogitems
            var root = (IQueryable<CatalogItem>)_context.CatalogItems;
            //we are checking that if my catalogtype id is having null or value
            if(catalogTypeId.HasValue)
            {
                //because user is filtering for catalog type so that is y we are using where like sql query
                root = root.Where(c => c.CatalogTypeId == catalogTypeId);
            }
            if(catalogBrandId.HasValue)
            {
                root = root.Where(c => c.CatalogBrandId == catalogBrandId);
            }

            //giving the count based on the root. root is the actual query filtering our types and brands
            var itemsCount = await root.LongCountAsync();

            //using the root to skip and take from that
            //root contains actual query
            //we need to put orderby first because of the table names
            var items = await root
                .OrderBy(c => c.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel<CatalogItem>
            { 
                PageIndex = pageIndex,
                PageSize = pageSize,
                Count = itemsCount,
                Data = items
            };
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



        //creating another api for catalog types. we are getting all the catalogtypes from database. If there will be any call to catalog types then this api will get called.
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            //we are calling the catalog types from our catalogcontext and defining tolist
           var items =  await _context.CatalogTypes.ToListAsync();
            //returning the response whether it is good or not.
            return Ok(items);
        }

        //creating another api for catalog brands. we are getting all the catalogbrands from database. If there will be any call to catalog brands then this api will get called.
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            //we are calling the catalog brands from our catalogcontext and defining tolist
            var items = await _context.CatalogBrands.ToListAsync();
            //returning the response whether it is good or not.
            return Ok(items);
        }
    }
}