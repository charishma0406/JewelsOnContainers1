using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OrderApi.Data;

namespace OrderApi
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
            services.AddControllers().AddNewtonsoftJson();
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

            //var connectionString = Configuration["ConnectionString"];

            //adding data base context and giving options method to connect my data base through the configuration
            services.AddDbContext<OrdersContext>(options => options.UseSqlServer(connectionstring));

            ConfigureAuthService(services);

            //adding swagger to generate swagger documentation for us and adding options to the swagger
            services.AddSwaggerGen(options =>
            {
                //swagger document it is we can tell what version our application.This is name of our version (V1)
                options.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    //title of our documentation
                    Title = "JewelsonContainer - Basket Api",
                    //this is the actual version
                    Version = "v1",
                    //description for my documentation
                    Description = "Basket service API",
                });
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //for security authentication
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    //type of flow of our identity.implicit is the way to talk to identity server to confirm the token
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            //this is the url if you need to authorize
                            AuthorizationUrl = new Uri($"{Configuration["IdentityUrl"]}/connect/authorize"),
                            //this is the url to identify the token
                            TokenUrl = new Uri($"{Configuration["IdentityUrl"]}/connect/token"),
                            //who is going to be calling this
                            Scopes = new Dictionary<string, string>
                            {
                                //basket name should match in the token server config file left side name
                                {"basket", "Basket Api"}

                            }


                        }
                    }

                });
            });

        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            //how we integrate identity with an application
            //we want to know identity server is, it located in configuration
            var identityUrl = Configuration["identityUrl"];
            //we need to add authentication
            services.AddAuthentication(options =>
            {
                // jwtbearer is a token askng about identity
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // challenge is for askng for user name and password for identity
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //telling where the identity server is located
            }).AddJwtBearer(options =>
            {
                //identityurl is issue the token
                options.Authority = identityUrl;
                //for security
                options.RequireHttpsMetadata = false;
                //we need to tell who you are. the name of the auience should match in our identity server
                options.Audience = "order";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
