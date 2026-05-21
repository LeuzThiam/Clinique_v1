using MaBoutique.Services.ApiModels;

namespace MaBoutique.Services
{
    public interface ICartApiClient
    {
        Task<CartApiModel?> GetCartAsync(int userId);
        Task<CartApiModel?> AddItemAsync(int userId, UpsertCartItemApiModel item);
        Task<bool> RemoveItemAsync(int userId, int productId);
        Task<bool> ClearCartAsync(int userId);
    }
}
