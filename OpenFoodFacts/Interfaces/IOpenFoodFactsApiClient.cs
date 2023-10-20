using OpenFoodFacts.DotNet.Wrapper.Models;

namespace OpenFoodFacts.DotNet.Wrapper.Interfaces
{
    public interface IOpenFoodFactsApiClient
    {
        Task<ProductResponse> FetchProductByCode(string code);
        Task<ProductResponse> FetchProductByName(string name);
    }
}
