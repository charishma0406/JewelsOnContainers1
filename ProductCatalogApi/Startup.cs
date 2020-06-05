using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductCatalogApi.Data;
using ProductCatalogApi.Domain;

namespace ProductCatalogApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // we changed connection string we took it away and we changed it to the paramaters in the yml file
            //we are defining those here as parameters in startuo so if we run docker then it will take these config
            var server = Configuration["DataBaseServer"];
            //database name
            var database = Configuration["DataBaseName"];
            //username
            var user = Configuration["DataBaseUser"];
            //password
            var password = Configuration["DataBasePassword"];
            //total connection string for sql server
            var connectionstring = $"Server = {server};DataBase = {database};User Id = {user}; Password = {password}";


            //adding data base context and giving options method to connect my data base through the configuration
            services.AddDbContext<CatalogContext>(options => 
            options.UseSqlServer(connectionstring));



            //adding swagger to generate swagger documentation for us and adding options to the swagger
            services.AddSwaggerGen(options => 
            {
                //swagger document it is we can tell what version our application.This is name of our version (V1)
                options.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    //title of our documentation
                    Title = "JewelsonContainer - Product Catalog Api",
                    //this is the actual version
                    Version = "v1",
                    //description for my documentation
                    Description = "Product catalog microservice",
                });
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            //we are telling our app to use swagger and adding swagger ui to see our documentation for certain endpoint
            //
            app.UseSwagger().UseSwaggerUI(e =>
            {
                //we will show under this url for swagger, version v1 and name for this file as product catalogapi
                e.SwaggerEndpoint($"/swagger/V1/swagger.json", "ProductCatalogAPI V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
