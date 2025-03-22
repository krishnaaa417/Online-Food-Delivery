using ePizza.Models.Request;

namespace ePizza.Core.Contracts
{
    public interface IOrderService
    {
        Task<string> AddOrdersAsync(OrderRequest order);

    }
}
