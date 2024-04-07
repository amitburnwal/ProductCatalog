using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace ProductCatalog.Api.Domain.Product
{
    public class ShopperHistory : ValueObject
    {
        public ShopperHistory(string customerId, List<Product> products)
        {
            CustomerId = customerId;
            Products = products;
        }

        public string CustomerId { get; private set; }
        //use using statement instead of full namespace identifier
        public List<Domain.Product.Product> Products { get; private set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CustomerId;
            yield return Products;
        }
    }
}