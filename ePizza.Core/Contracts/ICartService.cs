using ePizza.Models.Request;
using ePizza.Models.Response;

namespace ePizza.Core.Contracts
{
    public interface ICartService
    {
        Task<CartResponseModel> GetCartDetailsAsync(Guid cartId);  

        Task<bool> AddItemToCartAsync(AddToCartRequest request);   

        Task<bool> DeleteItemFromCartAsync(Guid cartId, int itemId);

        Task<bool> UpdateItemInCartAsync(Guid cartId, int itemId, int quantity);

        Task<int> GetItemCountAsync(Guid cartId);

        Task<int> UpdateCartUser(Guid cartId, int userId);
    }
}
