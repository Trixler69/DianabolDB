using System.Text.Json.Serialization;

namespace DataAccess.Model
{
    public class ProductsResponse
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
