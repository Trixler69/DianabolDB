using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Meal
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Ignore]
        public IEnumerable<MealRelation> SubMeals { get; set; } = Enumerable.Empty<MealRelation>();
        private double weight = 100;
        private double calories;
        private double protein;
        private double carbohydrates;
        private double fat;
        public double Calories
        {
            get
            {
                if (!SubMeals.Any()) { return calories; }
                double sum = 0;
                SubMeals.ToList().ForEach(m => sum += m.Calories);
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
                if (!SubMeals.Any()) { return protein; }
                double sum = 0;
                SubMeals.ToList().ForEach(m => sum += m.Protein);
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
                if (!SubMeals.Any()) { return carbohydrates; }
                double sum = 0;
                SubMeals.ToList().ForEach(m => sum += m.Carbohydrates);
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
                if (!SubMeals.Any()) { return fat; }
                double sum = 0;
                SubMeals.ToList().ForEach(m => sum += m.Fat);
                return sum;
            }
            set { fat = value; }
        }
        public double Weight
        {
            get
            {
                if (!SubMeals.Any()) { return weight; }
                double sum = 0;
                SubMeals.ToList().ForEach(m => sum += m.Amount);
                return sum;
            }
            set
            {
                weight = value;
            }
        }
    }

    public class MealRelation
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
        [Indexed]
        public int MealId { get; set; }
        [Indexed]
        public int SubMealId { get; set; }
        [Ignore]
        public Meal SubMeal { get; set; } = default!;
        public double Amount { get; set; }
        [Ignore]
        public double Calories => (SubMeal.Calories / SubMeal.Weight) * Amount;
        [Ignore]
        public double Protein => (SubMeal.Protein / SubMeal.Weight) * Amount;
        [Ignore]
        public double Carbohydrates => (SubMeal.Carbohydrates / SubMeal.Weight) * Amount;
        [Ignore]
        public double Fat => (SubMeal.Fat / SubMeal.Weight) * Amount;
    }
}
