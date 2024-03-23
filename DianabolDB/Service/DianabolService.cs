using DianabolDB.Data;
using JsonFlatFileDataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.Net.Http;
using DianabolDB.Interface;

namespace DianabolDB.Service
{
    public class DianabolService : IDianabolService
    {
        private readonly DataStore DataStore;
        private readonly IDocumentCollection<Meal> Meals;
        private readonly IDocumentCollection<Day> Days;
        private readonly Random Random = new();

        public DianabolService()
        {
            DataStore = new DataStore(Path.Combine(FileSystem.AppDataDirectory, "datastore.json"));
            Meals = DataStore.GetCollection<Meal>();
            Days = DataStore.GetCollection<Day>();

            //Meals.DeleteMany(x => true);
            //Days.DeleteMany(x => true);
            //InsertTestData();
        }

        public async void MergeMeal(Meal meal)
        {
            meal.IngredientObjects = null;
            if (meal.Id == null)
            {
                if (Meals.AsQueryable().Count() == 0)
                {
                    meal.Id = 1;
                }

                await Meals.InsertOneAsync(meal);
            }
            else
            {
                await Meals.ReplaceOneAsync(meal.Id, meal);
            }
        }

        public async Task MergeDay(Day day)
        {
            day.MealObjects = null;
            if (day.Id == null)
            {
                if (Days.AsQueryable().Count() == 0)
                {
                    day.Id = 1;
                }

                await Days.InsertOneAsync(day);
            }
            else
            {
                await Days.ReplaceOneAsync(day.Id, day);
            }
        }

        public async Task RemoveMeal(Meal meal)
        {
            var recipes = await GetRecipesAsync();
            var days = await GetDaysAsync();
            var recipesUsed = recipes.Where(r => r.Ingredients.Where(i => i.Value.Value == meal.Id).Any());
            var daysUsed = days.Where(d => d.Meals.Where(m => m.Value.Value == meal.Id || recipes.Where(r => r.Id == m.Value.Value).Any()).Any());

            if (recipesUsed.Any() || daysUsed.Any()) { throw new ArgumentException("Meal is already part of an Recipe or Day"); }

            await Meals.DeleteOneAsync(meal.Id);
        }

        public Task<IEnumerable<Meal>> GetIngredientsAsync()
        {
            return Task.FromResult(Meals.AsQueryable().Where(m => m.Ingredients == null));
        }

        public Task<IEnumerable<Meal>> GetRecipesAsync()
        {
            var meals = Meals.AsQueryable();

            return Task.FromResult(
                meals.Where(m => m.Ingredients != null)
                    .Select(r => new Meal
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        Ingredients = r.Ingredients,
                        IngredientObjects = r.Ingredients.Select(i => new KeyValuePair<int, KeyValuePair<int, Meal>>(i.Key, new KeyValuePair<int, Meal>(i.Value.Key, meals.FirstOrDefault(m => m.Id == i.Value.Value))))
                    })
            );
        }

        public async Task<IEnumerable<Day>> GetDaysAsync()
        {
            var meals = await GetMealsAsync();
            return Days.AsQueryable().Select(d => new Day
            {
                Id = d.Id,
                Date = d.Date,
                Weight = d.Weight,
                Meals = d.Meals,
                MealObjects = d.Meals.Select(i => new KeyValuePair<int, KeyValuePair<int, Meal>>(i.Key, new KeyValuePair<int, Meal>(i.Value.Key, meals.FirstOrDefault(m => m.Id == i.Value.Value))))
            });
        }

        public async Task<IEnumerable<Meal>> GetMealsAsync()
        {
            var recipes = await GetRecipesAsync();
            var ingredients = await GetIngredientsAsync();
            return recipes.Concat(ingredients);
        }
    }
}
