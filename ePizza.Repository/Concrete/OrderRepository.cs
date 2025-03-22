using ePizza.Domain.Models;
using ePizza.Repository.Contracts;

namespace ePizza.Repository.Concrete
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(EpizzaHubDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddNewOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            int rowsAffected = await _dbContext.SaveChangesAsync();

            return rowsAffected > 0;
        }
    }
}
