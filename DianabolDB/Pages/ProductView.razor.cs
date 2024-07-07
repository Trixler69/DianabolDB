using CommunityToolkit.Maui.Views;
using DataAccess.Model;
using DianabolDB.MauiPages;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianabolDB.Pages
{
    public partial class ProductView
    {
        private IEnumerable<Meal> Ingredients { get; set; }
        private Meal EditMeal { get; set; }
        private bool ShowEdit { get; set; }
        private string SearchString { get; set; }
        private bool ShowLoading { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void NewMeal(Meal meal)
        {
            EditMeal = meal;
            ShowEdit = true;
        }

        private void SaveMeal(Meal meal)
        {
            DianabolService.MergeMeal(meal);
            ShowEdit = false;
        }

        private async Task SearchProduct()
        {

            try
            {
                ShowLoading = true;
                Ingredients = (await OpenFoodFactsService.FetchProductByName(SearchString))?.Products.Select(p => new Meal()
                {
                    Weight = 100.0,
                    Name = p.Brands + " " + p.ProductName,
                    Calories = (p.Nutriments.EnergyKcal100g == null ? 0 : (Double)p.Nutriments.EnergyKcal100g),
                    Carbohydrates = (p.Nutriments.Carbohydrates100g == null ? 0 : (Double)p.Nutriments.Carbohydrates100g),
                    Fat = (p.Nutriments.Fat100g == null ? 0 : (Double)p.Nutriments.Fat100g),
                    Protein = (p.Nutriments.Proteins100g == null ? 0 : (Double)p.Nutriments.Proteins100g)
                });
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Product loading failed", Detail = ex.Message, Duration = 4000 });
            }
            ShowLoading = false;
        }

        private void OnWeightChange(double weight)
        {
            var factor = weight / EditMeal.Weight;
            EditMeal.Weight = weight;
            EditMeal.Calories = EditMeal.Calories * factor;
            EditMeal.Protein = EditMeal.Protein * factor;
            EditMeal.Carbohydrates = EditMeal.Carbohydrates * factor;
            EditMeal.Fat = EditMeal.Fat * factor;
        }

        private async void OpenPopup()
        {
            string code = "";
            var popupPage = new MauiPopupPage()
            {
                CanBeDismissedByTappingOutsideOfPopup = false
            };

            var result = await App.Current.MainPage.ShowPopupAsync(popupPage);

            if (result != null)
            {
                code = result?.ToString();
            }
            else
            {
                return;
            }

            try
            {
                var p = (await OpenFoodFactsService.FetchProductByCode(code))?.Product;
                var product = new Meal()
                {
                    Weight = 100,
                    Name = p.Brands + " " + p.ProductName,
                    Calories = (p.Nutriments.EnergyKcal100g == null ? 0 : (Double)p.Nutriments.EnergyKcal100g),
                    Carbohydrates = (p.Nutriments.Carbohydrates100g == null ? 0 : (Double)p.Nutriments.Carbohydrates100g),
                    Fat = (p.Nutriments.Fat100g == null ? 0 : (Double)p.Nutriments.Fat100g),
                    Protein = (p.Nutriments.Proteins100g == null ? 0 : (Double)p.Nutriments.Proteins100g)
                };
                NewMeal(product);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Product loading failed", Detail = ex.Message, Duration = 4000 });
            }

            StateHasChanged();
        }
    }
}
