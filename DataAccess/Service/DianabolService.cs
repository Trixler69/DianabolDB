using DataAccess.Interface;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public class DianabolService : IDianabolService
    {
        private readonly string databasePath;

        public DianabolService(string appDataDirectory)
        {
            databasePath = Path.Combine(appDataDirectory, Constants.DatabaseFilename);
            InitializeDB();
        }

        private SQLiteConnection BuildConnection()
        {
            return new SQLiteConnection(databasePath, Constants.Flags);
        }

        public IEnumerable<Meal> GetMeals()
        {
            string sql = $@"SELECT * FROM {nameof(Meal)}";

            using (var connection = BuildConnection())
            {
                var meals = connection.Query<Meal>(sql);

                meals.ForEach(meal => meal.SubMeals = GetMealRelations((int)meal.Id));

                return meals;
            }
        }

        public Meal? GetMeal(int mealId)
        {
            string sql = $@"SELECT * FROM {nameof(Meal)} where {nameof(Meal.Id)} == ?";

            using (var connection = BuildConnection())
            {
                return connection.Query<Meal>(sql, mealId).FirstOrDefault();
            }
        }

        public IEnumerable<MealRelation> GetMealRelations(int mealId)
        {
            string sql = $@"SELECT * FROM {nameof(MealRelation)} where {nameof(MealRelation.MealId)} == ?";

            using (var connection = BuildConnection())
            {
                var mealRels = connection.Query<MealRelation>(sql, mealId);

                mealRels.ForEach(relation =>
                {
                    var meal = GetMeal(relation.SubMealId);
                    if (meal == null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        relation.SubMeal = meal;
                    }
                });

                return mealRels;
            }
        }

        public IEnumerable<Day> GetDays()
        {
            string sql = $@"SELECT * FROM {nameof(Day)}";

            using (var connection = BuildConnection())
            {
                var days = connection.Query<Day>(sql);

                days.ForEach(day =>
                {
                    var relations = GetDayRelations(day.Id);
                    day.Meals = relations ?? Enumerable.Empty<DayRelation>();
                });

                return days;
            }
        }

        public IEnumerable<DayRelation>? GetDayRelations(int dayId)
        {
            string sql = $@"SELECT * FROM {nameof(DayRelation)} where {nameof(DayRelation.DayId)} == ?";

            using (var connection = BuildConnection())
            {
                var dayRels = connection.Query<DayRelation>(sql, dayId);

                dayRels.ForEach(relation =>
                {
                    var meal = GetMeal(relation.MealId);
                    if (meal == null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        relation.Meal = meal;
                    }
                });

                return dayRels;
            }
        }

        public IEnumerable<Meal> GetIngredients()
        {
            var x = GetMeals();
            return GetMeals().Where(m => !m.SubMeals.Any());
        }

        public IEnumerable<Meal> GetRecipes()
        {
            return GetMeals().Where(m => m.SubMeals.Any());
        }

        public void RemoveMeal(Meal meal)
        {
            using (var connection = BuildConnection())
            {
                connection.Delete(meal);
            }
        }

        public void MergeMeal(Meal meal)
        {
            using (var connection = BuildConnection())
            {
                connection.BeginTransaction();
                connection.InsertOrReplace(meal);
                var rowID = (int)connection.ExecuteScalar<long>("select last_insert_rowid()");
                connection.Commit();

                var id = meal.Id == null ? rowID : (int)meal.Id;

                meal.SubMeals.ToList().ForEach(m => m.MealId = id);

                if (meal.SubMeals.Any())
                {
                    var dbRelations = GetMealRelations(id)?.ToList();

                    dbRelations?.ForEach(r =>
                        {
                            if (!meal.SubMeals.Where(m => m.Id == r.Id).Any())
                            {
                                connection.Delete(r);
                            }
                        });


                    meal.SubMeals.ToList().ForEach(m => connection.InsertOrReplace(m));
                }
            }
        }

        public void MergeMealRelation(MealRelation relation)
        {
            using (var connection = BuildConnection())
            {
                connection.InsertOrReplace(relation);
            }
        }

        public void MergeDay(Day day)
        {
            using (var connection = BuildConnection())
            {
                connection.InsertOrReplace(day);
            }
        }

        public void MergeDayRelation(DayRelation relation)
        {
            using (var connection = BuildConnection())
            {
                connection.InsertOrReplace(relation);
            }
        }

        public void RemoveDayRelation(DayRelation relation)
        {
            using (var connection = BuildConnection())
            {
                connection.Delete(relation);
            }
        }

        private void InitializeDB()
        {
            using (var connection = BuildConnection())
            {
                connection.DropTable<Meal>();
                connection.DropTable<MealRelation>();
                connection.DropTable<Day>();
                connection.DropTable<DayRelation>();
                connection.CreateTable<Meal>();
                connection.CreateTable<MealRelation>();
                connection.CreateTable<Day>();
                connection.CreateTable<DayRelation>();
            }
        }
    }

    public static class Constants
    {
        public const string DatabaseFilename = "datastore.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;
    }
}
