using DianabolDB.Data;
using DianabolDB.Interface;
using System.Net.Http;
using System.Text.Json;

namespace DianabolDB.Service
{
    public class OpenFoodFactsService : IOpenFoodFactsService
    {
        private const string API_URL = "https://world.openfoodfacts.org/cgi/search.pl?";

        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public OpenFoodFactsService(IHttpClientFactory clientFactory, string apiUrl = default)
        {
            _httpClient = clientFactory.CreateClient();
            _apiUrl = apiUrl;
            if (string.IsNullOrEmpty(apiUrl))
            {
                _apiUrl = API_URL;
            }
        }
        public OpenFoodFactsService(string apiUrl = default)
        {
            _httpClient = new HttpClient();
            _apiUrl = apiUrl;
            if (string.IsNullOrEmpty(apiUrl))
            {
                _apiUrl = API_URL;
            }
        }

        public OpenFoodFactsService(HttpClient client, string apiUrl = default)
        {
            _httpClient = client;
            _apiUrl = apiUrl;
            if (string.IsNullOrEmpty(apiUrl))
            {
                _apiUrl = API_URL;
            }
        }

        public async Task<ProductResponse> FetchProductByCode(string code)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/product/{code}.json");
            var stringContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ProductResponse>(stringContent);
            }
            throw new Exception(stringContent);
        }

        public async Task<ProductResponse> FetchProductByName(string name)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}search_terms={name}&search_simple=1&action=process&json=1");
            var stringContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ProductResponse>(stringContent);
            }
            throw new Exception(stringContent);
        }
    }
}
