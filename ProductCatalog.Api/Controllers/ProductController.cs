using System.Collections.Generic;
using System.Data.OracleClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Domain.HttpClients;
using ProductCatalog.Api.Domain.Product;

namespace ProductCatalog.Api.Controllers
{
    [Route("/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet("/sort")]
        public async Task<ActionResult<IEnumerable<Product>>> Sort([FromQuery] string sortOption)
        {

            //instead of creating the object of ProductHttpClient or ShopperHistoryHttpClient We should inject the respective interfaces to the constructor of the class
            // Or
            // we can think of implementing Mediator Pattern using nuget package like MediatR which decouples our Handlers from Controllers. We can inject the IMediatR interface and call the Send method of it by passing the Handler class object. The Service class to call the ProductHttpClient or ShopperHistoryClient should be injected in Handler. I am thinking in this direction because Class names (GetSortedProductQueryHandler) are termed as Verb instead of Noun which forced me think it is CQRS pattern.

            //We can have one custom Base controller to inject any common code we wanted to run for all the controller may be an Exception filters or any application events.
            //No Try catch written or handling bad respons as only positive scenario is being returned.
            //Http Call should not happen from controller instead some product service should be injected.


            var sortedProductQueryHandler =
                new GetSortedProductQueryHandler(new ProductHttpClient(), new ShopperHistoryHttpClient());
            
            var getSortedProductQuery = new GetSortedProductQuery(sortOption);
            var queryResponse = await sortedProductQueryHandler.Handle(getSortedProductQuery);
            return Ok(queryResponse.Products);
        }
    }
}