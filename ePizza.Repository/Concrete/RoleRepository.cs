using ePizza.Domain.Models;
using ePizza.Repository.Contracts;

namespace ePizza.Repository.Concrete
{
    public class RoleRepository : GenericRepository<Role>, IRolesRepository
    {
        // crud
        public RoleRepository(EpizzaHubDbContext dbContext) : base(dbContext)
        {
        }
    }
}
