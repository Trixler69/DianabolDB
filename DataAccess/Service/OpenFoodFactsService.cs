using DataAccess.Model;
using DataAccess.Interface;
using System.Net.Http;
using System.Text.Json;

namespace DataAccess.Service
{
    public class OpenFoodFactsService : IOpenFoodFactsService
    {
        private const string API_URL = "https://world.openfoodfacts.org/cgi/search.pl?";

        private readonly HttpClient httpClient;
        private readonly string apiUrl;


        public OpenFoodFactsService(string? apiUrl = default)
        {
            httpClient = new HttpClient();
            if (string.IsNullOrEmpty(apiUrl))
            {
                this.apiUrl = API_URL;
            }
            else
            {
                this.apiUrl = apiUrl ?? String.Empty;
            }
        }

        public async Task<ProductResponse?> FetchProductByCode(string code)
        {
            var response = await httpClient.GetAsync($"https://world.openfoodfacts.org/api/v2/product/{code}.json");
            var stringContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ProductResponse>(stringContent);
            }
            throw new Exception(stringContent);
        }

        public async Task<ProductsResponse?> FetchProductByName(string name)
        {
            var response = await httpClient.GetAsync($"{apiUrl}search_terms={name}&search_simple=1&action=process&json=1");
            var stringContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ProductsResponse>(stringContent);
            }
            throw new Exception(stringContent);
        }

    }
}
