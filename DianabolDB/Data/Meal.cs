using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianabolDB.Data
{
    public class Meal
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<KeyValuePair<int, KeyValuePair<int, int>>> Ingredients { get; set; }
        public IEnumerable<KeyValuePair<int, KeyValuePair<int, Meal>>> IngredientObjects { get; set; }

        private int weight = 100;
        private double calories;
        private double protein;
        private double carbohydrates;
        private double fat;
        public double Calories
        {
            get
            {
                if (IngredientObjects == null) { return calories; }
                double sum = 0;
                IngredientObjects.ToList().ForEach(i => sum += ((i.Value.Value.Calories / i.Value.Value.Weight) * i.Value.Key));
                return sum;
            }
            set
            {
                calories = value;
            }

        }
        public double Protein
        {
            get
            {
                if (IngredientObjects == null) { return protein; }
                double sum = 0;
                IngredientObjects.ToList().ForEach(i => sum += ((i.Value.Value.Protein / i.Value.Value.Weight) * i.Value.Key));
                return sum;
            }
            set
            {
                protein = value;
            }
        }
        public double Carbohydrates
        {
            get
            {
                if (IngredientObjects == null) { return carbohydrates; }
                double sum = 0;
                IngredientObjects.ToList().ForEach(i => sum += ((i.Value.Value.Carbohydrates / i.Value.Value.Weight) * i.Value.Key));
                return sum;
            }
            set 
            { 
                carbohydrates = value;
            }
        }
        public double Fat
        {
            get
            {
                if (IngredientObjects == null) { return fat; }
                double sum = 0;
                IngredientObjects.ToList().ForEach(i => sum += ((i.Value.Value.Fat / i.Value.Value.Weight) * i.Value.Key));
                return sum;
            }
            set { fat = value; }
        }
        public int Weight
        {
            get
            {
                if (IngredientObjects == null) { return weight; }
                int sum = 0;
                IngredientObjects.ToList().ForEach(i => sum += i.Value.Key);
                return sum;
            }
            set 
            { 
                weight = value; 
            }
        }
    }
}
