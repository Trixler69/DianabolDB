using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class Product
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; } = string.Empty;
        [JsonPropertyName("product_quantity")]
        public string? ProductQuantity { get; set; } = string.Empty;
        [JsonPropertyName("nutriments")]
        public Nutriments? Nutriments { get; set; }
        [JsonPropertyName("brands")]
        public string Brands { get; set; } = string.Empty;
    }
}
