using DataAccess.Model;

namespace DataAccess.Interface
{
    public interface IOpenFoodFactsService
    {
        Task<ProductResponse?> FetchProductByCode(string code);
        Task<ProductsResponse?> FetchProductByName(string name);
    }
}