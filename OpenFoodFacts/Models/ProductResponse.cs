using System.Text.Json.Serialization;

namespace OpenFoodFacts.DotNet.Wrapper.Models
{
    public class ProductResponse
    {
        

        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }
        

    }
}
