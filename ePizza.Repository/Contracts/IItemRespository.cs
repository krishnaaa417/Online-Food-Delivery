using ePizza.Domain.Models;
using ePizza.Domain.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Repository.Contracts
{
    public interface IItemRespository : IGenericRepository<Item>
    {


        List<GetOrderDetailsDTO> CallProcedure();
    }
}
