using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DataAccess.Model
{
    public class Day
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
        public DateTime Date { get; set; }
        [Ignore]
        public IEnumerable<DayRelation> Meals { get; set; } = Enumerable.Empty<DayRelation>();
        public double? BodyWeight { get; set; }
        [Ignore]
        public double Calories
        {
            get
            {
                double sum = 0;
                Meals.ToList().ForEach(m => sum += m.Calories);
                return sum;
            }
        }
        [Ignore]
        public double Protein
        {
            get
            {
                double sum = 0;
                Meals.ToList().ForEach(m => sum += m.Protein );
                return sum;
            }
        }
        [Ignore]
        public double Carbohydrates
        {
            get
            {
                double sum = 0;
                Meals.ToList().ForEach(m => sum += m.Carbohydrates);
                return sum;
            }
        }
        [Ignore]
        public double Fat
        {
            get
            {
                double sum = 0;
                Meals.ToList().ForEach(m => sum += m.Fat);
                return sum;
            }
        }
    }

    public class DayRelation
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
        [Indexed]
        public int DayId { get; set; }
        [Indexed]
        public int MealId { get; set; }
        [Ignore]
        public Meal Meal { get; set; } = default!;
        public double RelAmount { get; set; }
        [Ignore]
        public double Amount => RelAmount * Meal.Weight;
        [Ignore]
        public double Calories => Meal.Calories * RelAmount;
        [Ignore]
        public double Protein => Meal.Protein * RelAmount;
        [Ignore]
        public double Carbohydrates => Meal.Carbohydrates * RelAmount;
        [Ignore]
        public double Fat => Meal.Fat * RelAmount;
    }
}
