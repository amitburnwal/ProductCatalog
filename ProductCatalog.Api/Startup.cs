using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductCatalog.Api.Domain.HttpClients;
using ProductCatalog.Api.Domain.Product;

namespace ProductCatalog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

         
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<GetSortedProductQueryHandler>();
            //We can use scope instead of Transient for service object life.
            services.AddTransient<IProductHttpClient, ProductHttpClient>();
            services.AddTransient<IShopperHistoryHttpClient, ShopperHistoryHttpClient>();
        }

        //We can have any Filters like Exceptionfilter or Authentication Filter injected in middleware.
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
                endpoints.Map("/", async context => { await context.Response.WriteAsync("Welcome to Product Catalog API"); });
                
                endpoints.Map("/trolleyTotal", async context => { await WooliesXProxy.TrolleyCalculator(context); });

                endpoints.MapControllers();
            });
        }
    }
}