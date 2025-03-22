using ePizza.Domain.Models;
using ePizza.Repository.Contracts;

namespace ePizza.Repository.Concrete
{
    public class PaymentRepository : GenericRepository<PaymentDetail>, IPaymentRepository
    {
        public PaymentRepository(EpizzaHubDbContext dbContext) : base(dbContext)
        {
        }
    }
}
