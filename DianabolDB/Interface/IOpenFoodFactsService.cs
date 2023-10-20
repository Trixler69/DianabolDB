using DianabolDB.Data;

namespace DianabolDB.Interface
{
    public interface IOpenFoodFactsService
    {
        Task<ProductResponse> FetchProductByCode(string code);
        Task<ProductResponse> FetchProductByName(string name);
    }
}