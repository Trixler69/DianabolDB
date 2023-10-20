using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianabolDB.Data
{
    public class Day
    {
        public int? Id { get; set; }
        public DateTime Date { get; set; }
        public Double? Weight { get; set; }
        public IEnumerable<KeyValuePair<int, KeyValuePair<int, int>>> Meals { get; set; } = new List<KeyValuePair<int, KeyValuePair<int, int>>>();
        public IEnumerable<KeyValuePair<int, KeyValuePair<int, Meal>>> MealObjects { get; set; } = new List<KeyValuePair<int, KeyValuePair<int, Meal>>>();
        public double Calories
        {
            get
            {
                if (MealObjects == null) { return 0; }
                double calories = 0;
                MealObjects.ToList().ForEach(i => calories += ((i.Value.Value.Calories / i.Value.Value.Weight) * i.Value.Key));
                return calories;
            }
        }
        public double Protein
        {
            get
            {
                if (MealObjects == null) { return 0; }
                double sum = 0;
                MealObjects.ToList().ForEach(i => sum += ((i.Value.Value.Protein / i.Value.Value.Weight) * i.Value.Key));
                return sum;
            }
        }
        public double Carbohydrates
        {
            get
            {
                if (MealObjects == null) { return 0; }
                double sum = 0;
                MealObjects.ToList().ForEach(i => sum += ((i.Value.Value.Carbohydrates / i.Value.Value.Weight) * i.Value.Key));
                return sum;
            }
        }
        public double Fat
        {
            get
            {
                if (MealObjects == null) { return 0; }
                double sum = 0;
                MealObjects.ToList().ForEach(i => sum += ((i.Value.Value.Fat / i.Value.Value.Weight) * i.Value.Key));
                return sum;
            }
        }
    }
}
