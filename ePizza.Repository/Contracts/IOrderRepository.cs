using ePizza.Domain.Models;

namespace ePizza.Repository.Contracts
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<bool> AddNewOrder(Order order);    
    }
}
