using ePizza.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Core.Contracts
{
    public interface IPaymentService
    {

        string MakePayment(MakePaymentRequest paymentRequest);

    }
}
