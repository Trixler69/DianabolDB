using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DianabolDB.Data
{
    public class Product
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
        [JsonPropertyName("nutriments")]
        public Nutriments Nutriments { get; set; }
        [JsonPropertyName("brands")]
        public string Brands { get; set; }
    }
}
