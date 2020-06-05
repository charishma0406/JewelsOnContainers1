using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderApi.Data;

namespace OrderApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //building the host 
            var host = CreateHostBuilder(args).Build();
            //we are calling our seed class here because of the sql server it is up and running we dono when it will start to call in start up file
            // to check whether the context is up and running so we are writing using method here
            using (var scope = host.Services.CreateScope())
            {
                //providing that which service provider it is, here it is catalog context is the service provider
                var serviceproviders = scope.ServiceProvider;
                //this line is basically saying that service provider can you tell me eventcontext is up and running.
                var context = serviceproviders.GetRequiredService<OrdersContext>();
                //if my db is available and up and running then call the seed method here.
                MigrateDataBase.EnsureCreated(context);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
