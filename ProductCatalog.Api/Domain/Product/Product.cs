using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProductCatalog.Api.Domain.Product
{
    //Can we use Record type here.
    // Benefits: In that case we will achieve Value Object features
    // concise and readable code using Primary Constructor 
    // Benefits of Record type inbuilt features like With, Object Representation or ToString method, 
    public class Product : ValueObject
    {
        [JsonProperty] public string Name { get; }
        [JsonProperty] public double Price { get; }
        [JsonProperty] public double Quantity { get; }

        public Product(string name, double price, double quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Name;
            yield return Price;
            yield return Quantity;
        }
    }
}