using System;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProductCatalog.Api.Domain.HttpClients
{
    //instead of writing the httpClient here, I would like to create a service layer so that sepration of concerns should be there. Right now it is tighly coupled. 
    // Any dependency like logger should be injected here in constructor. 
    public interface IProductHttpClient
    {
        Task<IEnumerable<Product.Product>> GetProducts();
    }

    public class ProductHttpClient : IProductHttpClient
    {
        //url should be in app.setting json file.
        private readonly string _productUrl = "http://dev-wooliesx-recruitment.azurewebsites.net/api/resource/products";

        //There should be some DTO instead of using Product Entity directly. 
        public async Task<IEnumerable<Product.Product>> GetProducts()
        {
            try
            {
                var products = await _productUrl
                    //passing the session token in query param is unsafe. we can think of using JWT for API Authentication which is more secure and self contained and performance wise good. 
                    .SetQueryParam("token", "25a4f06f-8fd5-49b3-a711-c013c156f8c8")
                    .AllowAnyHttpStatus()
                    .WithHeader("Accept", "application/json")
                    .GetJsonAsync<Product.Product[]>();
                return products;
            }
            catch (Exception e)
            {
                //logging of exception
                Console.WriteLine(e);
                throw;
            }
        }
    }
}