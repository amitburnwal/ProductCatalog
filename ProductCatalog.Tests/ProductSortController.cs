using Xunit;
using Newtonsoft.Json;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProductCatalog.Tests.DataHelpers;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductCatalog.Api.Domain.Product;

namespace ProductCatalog.Tests
{
    //the test classes are hitting the actual end points rightnow. 
    //the test classes are not structured with dependency injections. So tomorrow if we need to substitute the dependencies it will be difficult. 
    // we should create _sut (system under testing object) and Pass the references of dependencies to mock library and intilize them in constructor
    // we might also want to setup the mock for different test scenarios. 
    public class ProductSortController
    {
        
        [Fact]
        public async Task SortEndpointIsConfiguredAndReturnsCorrectJsonResponse()
        {
            // Arrange
            var httpClient = new WebApplicationFactory<ProductCatalog.Api.Startup>().Server.CreateClient();
            
            // Act
            var httpResponseMessage = await httpClient.GetAsync("/sort?sortOption=High");
            
            // Assert
            httpResponseMessage.StatusCode.Should().Be(StatusCodes.Status200OK);
            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<Product[]>(readAsStringAsync);
            products.Should().Equal(ListOfProduct.SortedProductsFormHighToLow);
        }
        
        [Fact]
        public async Task SortEndpointIsConfiguredAndReturnsCorrectJsonResponseForRecommended()
        {
            // Arrange
            var httpClient = new WebApplicationFactory<ProductCatalog.Api.Startup>().Server.CreateClient();
            
            // Act
            var httpResponseMessage = await httpClient.GetAsync("/sort?sortOption=Recommended");
            
            // Assert
            httpResponseMessage.StatusCode.Should().Be(StatusCodes.Status200OK);
            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<Product[]>(readAsStringAsync);
            products.Should().Equal(ListOfProduct.SortedBasedOnRecommended);
        }
    }
}