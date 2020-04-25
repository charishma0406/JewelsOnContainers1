using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class PicController : ControllerBase
    {
        //readonly declaration
        //iwebenvironment is used here for which env we are using there are 3 env like docker and iss and poershell commands
        //so here we are using iis express to find the env
        private readonly IWebHostEnvironment _env;
        //creating constructor for dependency injection, injecting iwebhost
        //env becz to see where our host(for pics) is located
        public PicController(IWebHostEnvironment env)
        {
            //assigning a value for readonly declaration
            _env = env;
        }

        //http get method and passing id as parameter
        [HttpGet("{id}")]
        public IActionResult GetImage(int id)
        {
            //webroot path
            var webRoot = _env.WebRootPath;
            //combining the webroot path and pics
            var path =  Path.Combine($"{webRoot}/Pics/",$"Ring{id}.jpg");
            //reading all the files from that path and storing in the buffer variable
            var buffer=  System.IO.File.ReadAllBytes(path);
            //returning the buffer with the image content
            return File(buffer, "image/jpeg");
            

        }
    }
}