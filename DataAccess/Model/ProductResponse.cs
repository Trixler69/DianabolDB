using System.Text.Json.Serialization;

namespace DataAccess.Model
{
    public class ProductResponse
    {
        [JsonPropertyName("product")]
        public Product Product { get; set; } = new Product();
    }
}
