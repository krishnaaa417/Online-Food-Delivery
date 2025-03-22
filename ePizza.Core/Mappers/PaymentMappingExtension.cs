using AutoMapper;
using ePizza.Domain.Models;
using ePizza.Models.Request;

namespace ePizza.Core.Mappers
{
    public class PaymentMappingExtension : Profile
    {
        public PaymentMappingExtension()
        {
            CreateMap<MakePaymentRequest, PaymentDetail>();
            CreateMap<OrderRequest, Order>();
            CreateMap<OrderItems, OrderItem>();
        }
    }
}
