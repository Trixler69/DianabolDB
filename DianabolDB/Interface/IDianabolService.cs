using DianabolDB.Data;

namespace DianabolDB.Interface
{
    public interface IDianabolService
    {
        Task<IEnumerable<Day>> GetDaysAsync();
        Task<IEnumerable<Meal>> GetIngredientsAsync();
        Task<IEnumerable<Meal>> GetMealsAsync();
        Task<IEnumerable<Meal>> GetRecipesAsync();
        Task MergeDay(Day day);
        void MergeMeal(Meal meal);
        Task RemoveMeal(Meal meal);
    }
}