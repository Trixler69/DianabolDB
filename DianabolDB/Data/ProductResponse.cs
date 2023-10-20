using System.Text.Json.Serialization;

namespace DianabolDB.Data
{
    public class ProductResponse
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }
    }
}
