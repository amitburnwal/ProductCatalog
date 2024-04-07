using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProductCatalog.Api.Domain.HttpClients;

namespace ProductCatalog.Api.Domain.Product
{
    //Can we use Mediator Pattern here
    public class GetSortedProductQuery
    {
        public string SortOption { get; }

        public GetSortedProductQuery(string sortOption)
        {
            SortOption = sortOption;
        }
    }

    public class GetSortedProductQueryHandler
    {
        private readonly IProductHttpClient _productHttpClient;
        private readonly IShopperHistoryHttpClient _shopperHistoryHttpClient;


        public GetSortedProductQueryHandler(
            IProductHttpClient productHttpClient,
            IShopperHistoryHttpClient shopperHistoryHttpClient)
        {
            _productHttpClient = productHttpClient;
            _shopperHistoryHttpClient = shopperHistoryHttpClient;
        }

        public async Task<GetSortedProductQueryResponse> Handle(GetSortedProductQuery getSortedProductQuery)
        {
            var products = await _productHttpClient.GetProducts();
            var productList = products.ToList();
            //If else messy code. Can move to Switch Case for better readability            
            //Moreover the logic to sort the product should move to a method instead of here for readability and extensibility purpose. if Filtering of product is required then we can think of moving the logic to api itself for performance boost.
            // String comparision instead should go for Enum values check for typo errors.
            // The recommend method should be delegated to a service call. 
            if (getSortedProductQuery.SortOption == "Low")
                return new GetSortedProductQueryResponse(productList.OrderBy(product => product.Price));
            else if (getSortedProductQuery.SortOption == "High")
                return new GetSortedProductQueryResponse(productList.OrderByDescending(product => product.Price));
            else if (getSortedProductQuery.SortOption == "Ascending")
                return new GetSortedProductQueryResponse(productList.OrderBy(product => product.Name));
            else if (getSortedProductQuery.SortOption == "Descending")
                return new GetSortedProductQueryResponse(productList.OrderByDescending(product => product.Name));
            else if (getSortedProductQuery.SortOption == "Recommended")
                return new GetSortedProductQueryResponse(await Recommend(productList));
            else
                return new GetSortedProductQueryResponse(productList);
        }

        private async Task<IEnumerable<Product>> Recommend(IEnumerable<Product> products)
        {
            var shopperHistory = _shopperHistoryHttpClient.GetShopperHistory().Result;

            var productsOrderedBasedOnNumberOfOrders = from shoppingHistory in shopperHistory
                let allOrders = shoppingHistory.Products
                from order in allOrders
                group order by order.Name into ordersGroupedByName
                let productsAndNumberOfOrders =new
                {
                    NumberOfOrders = ordersGroupedByName.Sum(product => product.Quantity),
                    Product = products.SingleOrDefault(product => product.Name == ordersGroupedByName.Key)
                }  
                orderby productsAndNumberOfOrders.NumberOfOrders descending 
                select productsAndNumberOfOrders.Product;
            
            var orderedProducts = productsOrderedBasedOnNumberOfOrders.ToList();
            var productsThatWereNotOrdered = products.Except(orderedProducts);
            orderedProducts.AddRange(productsThatWereNotOrdered);
            return orderedProducts;
        }
    }


    public class GetSortedProductQueryResponse
    {
        //We should not use entity directly instead some viewModel or DTO for the Product and use Automapper to map the objects.
        public GetSortedProductQueryResponse(IEnumerable<Product> products)
        {
            Products = products;
        }

        public IEnumerable<Product> Products { get; }
    }
}