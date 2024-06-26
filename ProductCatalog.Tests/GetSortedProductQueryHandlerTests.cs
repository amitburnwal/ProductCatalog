using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using ProductCatalog.Tests.Stubs;
using ProductCatalog.Tests.DataHelpers;
using ProductCatalog.Api.Domain.Product;

namespace ProductCatalog.Tests
{
    //the test classes are not structured with dependency injections. So tomorrow if we need to substitute the dependencies it will be difficult. 
    // we should create _sut (system under testing objects) and Pass the references of dependency to mock library and intilize them in constructor
    // we also need to setup the mock also for different test scenarios. 
    //Include other test cases like negative test cases etc.
    public class GetSortedProductQueryHandlerTests
    {
        [Fact]
        public async Task ShouldReturnEmptyListOfProducts()
        {
            // Arrange
            var getSortedProductQuery = new GetSortedProductQuery("Low");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithEmptyProducts(),
                StubShopperHistoryHttpClient.WithNoHistory());
            
            // Act
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            
            // Assert
            getSortedProductQueryResponse.Products.Should().BeEmpty();
        }


        [Fact]
        public async Task ShouldReturnSortedFromLowToHigh()
        {
            // Arrange
            var getSortedProductQuery = new GetSortedProductQuery("Low");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh),
                StubShopperHistoryHttpClient.WithNoHistory());
            
            // Act
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);
            
            // Assert
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedProductsFormLowToHigh);
        }


        [Fact]
        public async Task ShouldReturnSortedFromHighToLow()
        {
            // Arrange
            var getSortedProductQuery = new GetSortedProductQuery("High");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh),
                StubShopperHistoryHttpClient.WithNoHistory());

            // Act
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);

            // Assert
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedProductsFormHighToLow);
        }
        
        [Fact]
        public async Task ShouldReturnSortedAscending()
        {
            // Arrange
            var getSortedProductQuery = new GetSortedProductQuery("Ascending");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh),
                StubShopperHistoryHttpClient.WithNoHistory());

            // Act
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);

            // Assert
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedAscending);
        }
        
        [Fact]
        public async Task ShouldReturnSortedDescending()
        {
            // Arrange
            var getSortedProductQuery = new GetSortedProductQuery("Descending");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh), 
                StubShopperHistoryHttpClient.WithNoHistory());

            // Act
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);

            // Assert
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedDescending);
        }
        
        [Fact]
        public async Task ShouldReturnSortedRecommended()
        {
            // Arrange
            var getSortedProductQuery = new GetSortedProductQuery("Recommended");
            var getSortedProductQueryHandler = new GetSortedProductQueryHandler(
                StubProductsHttpClient.WithProducts(ListOfProduct.ANotSortedProductsFormLowToHigh),
                StubShopperHistoryHttpClient.WithHistory(ShopperHistoryData.History));

            // Act
            var getSortedProductQueryResponse = await getSortedProductQueryHandler.Handle(getSortedProductQuery);

            // Assert
            getSortedProductQueryResponse.Products.Should().Equal(ListOfProduct.SortedBasedOnRecommended);
        }
    }
}