using DataAccess.Interface;
using DataAccess.Model;
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

                meals.ForEach(meal => meal.SubMeals = GetMealRelations(meal.Id));
                return meals;
            }
        }

        private Meal GetMeal(int mealId)
        {
            string sql = $@"SELECT * FROM {nameof(Meal)} where {nameof(Meal.Id)} == ?";

            using (var connection = BuildConnection())
            {
                return connection.Query<Meal>(sql, mealId).FirstOrDefault() ?? new Meal();
            }
        }

        private IEnumerable<MealRelation> GetMealRelations(int? mealId)
        {
            if (mealId == null) { return Enumerable.Empty<MealRelation>(); }

            string sql = $@"SELECT * FROM {nameof(MealRelation)} where {nameof(MealRelation.MealId)} == ?";

            using (var connection = BuildConnection())
            {
                var mealRels = connection.Query<MealRelation>(sql, mealId);

                mealRels.ForEach(relation =>
                {
                    relation.SubMeal = GetMeal(relation.SubMealId);
                });

                return mealRels;
            }
        }

        private IEnumerable<MealRelation> GetMealRelationsBySubMealId(int? mealId)
        {
            if (mealId == null) { return Enumerable.Empty<MealRelation>(); }

            string sql = $@"SELECT * FROM {nameof(MealRelation)} where {nameof(MealRelation.SubMealId)} == ?";

            using (var connection = BuildConnection())
            {
                return connection.Query<MealRelation>(sql, mealId);
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
                    day.Meals = GetDayRelations(day.Id);
                });

                return days;
            }
        }

        private IEnumerable<DayRelation> GetDayRelations(int? dayId)
        {
            if (dayId == null) { return Enumerable.Empty<DayRelation>(); }

            string sql = $@"SELECT * FROM {nameof(DayRelation)} where {nameof(DayRelation.DayId)} == ?";

            using (var connection = BuildConnection())
            {
                var dayRels = connection.Query<DayRelation>(sql, dayId);

                dayRels.ForEach(relation =>
                {
                    relation.Meal = GetMeal(relation.MealId);
                });

                return dayRels;
            }
        }

        private IEnumerable<DayRelation> GetDayRelationsByMealId(int? mealId)
        {
            if (mealId == null) { return Enumerable.Empty<DayRelation>(); }

            string sql = $@"SELECT * FROM {nameof(DayRelation)} where {nameof(DayRelation.MealId)} == ?";

            using (var connection = BuildConnection())
            {
                return connection.Query<DayRelation>(sql, mealId);
            }
        }

        public IEnumerable<Meal> GetIngredients()
        {
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
                if (GetMealRelationsBySubMealId(meal.Id).Any() || GetDayRelationsByMealId(meal.Id).Any())
                {
                    throw new InvalidOperationException("Delete Parent Meal or Day");
                }
                else
                {
                    connection.Delete(meal);
                    meal.SubMeals.ToList().ForEach(m => connection.Delete(m));
                }
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

        private void MergeMealRelation(MealRelation relation)
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
                connection.BeginTransaction();
                connection.InsertOrReplace(day);
                var rowID = (int)connection.ExecuteScalar<long>("select last_insert_rowid()");
                connection.Commit();

                var id = day.Id == null ? rowID : (int)day.Id;

                day.Meals.ToList().ForEach(m => m.DayId = id);

                if (day.Meals.Any())
                {
                    var dbRelations = GetDayRelations(id)?.ToList();

                    dbRelations?.ForEach(r =>
                    {
                        if (!day.Meals.Where(m => m.Id == r.Id).Any())
                        {
                            connection.Delete(r);
                        }
                    });

                    day.Meals.ToList().ForEach(m => connection.InsertOrReplace(m));
                }
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
                //connection.DropTable<Meal>();
                //connection.DropTable<Day>();
                //connection.DropTable<MealRelation>();
                //connection.DropTable<DayRelation>();

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
