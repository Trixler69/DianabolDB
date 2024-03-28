using DataAccess.Model;

namespace DataAccess.Interface
{
    public interface IDianabolService
    {
        IEnumerable<Day> GetDays();
        IEnumerable<Meal> GetMeals();
        IEnumerable<Meal> GetRecipes();
        IEnumerable<Meal> GetIngredients();
        void MergeDay(Day day);
        void MergeMeal(Meal meal);
        void RemoveMeal(Meal meal);
        void MergeDayRelation(DayRelation relation);
        void RemoveDayRelation(DayRelation relation);
    }
}