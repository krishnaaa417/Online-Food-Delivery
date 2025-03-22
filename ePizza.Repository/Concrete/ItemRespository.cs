using ePizza.Domain.Models;
using ePizza.Domain.StoredProcedures;
using ePizza.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ePizza.Repository.Concrete
{
    public class ItemRespository : GenericRepository<Item>, IItemRespository
    {
        public ItemRespository(EpizzaHubDbContext dbContext) : base(dbContext)
        {
        }

        public List<GetOrderDetailsDTO> CallProcedure()
        {
            var response = _dbContext.Database.SqlQueryRaw<GetOrderDetailsDTO>("exec sp_GetOrderDetails 'order_Q74yfsC1ABl1xc'").ToList();
            return response;
        }
    }
}
