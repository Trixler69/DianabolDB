using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class Nutriments
    {
        
        [JsonPropertyName("carbohydrates_100g")]
        public double? Carbohydrates100g { get; set; }

        [JsonPropertyName("energy-kcal_100g")]
        public double? EnergyKcal100g { get; set; }

        [JsonPropertyName("fat_100g")]
        public double? Fat100g { get; set; }

        [JsonPropertyName("proteins_100g")]
        public double? Proteins100g { get; set; }

    }
}
