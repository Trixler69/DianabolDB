using System.Text.Json.Serialization;

namespace DataAccess.Model
{
    public class ProductResponse
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
