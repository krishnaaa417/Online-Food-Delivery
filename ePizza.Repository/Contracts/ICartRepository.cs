using ePizza.Domain.Models;

namespace ePizza.Repository.Contracts
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetCartDetailsAsync(Guid cartId);

        Task<bool> DeleteItemAsync(Guid cartId, int itemId);

        Task<int> GetCartItemsQuantity(Guid cartId);

        Task<int> UpdateItemQuantity(Guid cartId,int itemId, int quantity);
    }
}
