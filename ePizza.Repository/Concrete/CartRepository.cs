using ePizza.Domain.Models;
using ePizza.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ePizza.Repository.Concrete
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(
            EpizzaHubDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> DeleteItemAsync(Guid cartId, int itemId)
        {
            var items = await _dbContext.CartItems.FirstOrDefaultAsync(x => x.CartId == cartId && x.ItemId == itemId);

            if (items != null)
            {
                _dbContext.CartItems.Remove(items);

                int recordsAffected = CommitChanges();

                return recordsAffected > 0;
            }

            return false;
        }

        public async Task<Cart> GetCartDetailsAsync(Guid cartId)
        {
            return await _dbContext
                        .Carts
                        .Include(x => x.CartItems)
                        .ThenInclude(x => x.Item)
                        .Where(
                               x => x.Id == cartId && x.IsActive == true)
                       .FirstOrDefaultAsync();
        }

        public async Task<int> GetCartItemsQuantity(Guid cartId)
        {
            return await _dbContext.CartItems.Where(x => x.CartId == cartId).CountAsync();
        }

        public async Task<int> UpdateItemQuantity(Guid cartId, int itemId, int quantity)
        {
            var currentItems = await _dbContext
                                            .CartItems
                                                .Where(x => x.CartId == cartId
                                                       && x.ItemId == itemId)
                                                .FirstOrDefaultAsync();

            currentItems.Quantity = quantity;
            _dbContext.Entry(currentItems).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }
    }
}
