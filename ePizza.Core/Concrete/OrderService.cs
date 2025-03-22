using ePizza.Core.Contracts;
using ePizza.Models.Request;
using ePizza.Repository.Contracts;

namespace ePizza.Core.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<string> AddOrdersAsync(OrderRequest order)
        {
            return await Task.FromResult("Rahul");
        }
    }
}
