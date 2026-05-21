using MaBoutique.Services.ApiModels;

namespace MaBoutique.Services
{
    public interface IOrderApiClient
    {
        Task<IReadOnlyList<OrderApiModel>> GetOrdersAsync(int userId);
        Task<OrderApiModel?> GetOrderAsync(int id);
        Task<OrderApiModel?> CreateOrderAsync(OrderApiModel order);
    }
}
