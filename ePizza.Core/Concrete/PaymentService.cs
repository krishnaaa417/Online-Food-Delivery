using AutoMapper;
using ePizza.Core.Contracts;
using ePizza.Domain.Models;
using ePizza.Models.Request;
using ePizza.Repository.Contracts;

namespace ePizza.Core.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public string MakePayment(MakePaymentRequest paymentRequest)
        {
            var paymentModel = _mapper.Map<PaymentDetail>(paymentRequest);

            if (paymentRequest.OrderRequest is not null 
                    && paymentRequest.OrderRequest.OrderItems.Count > 0)
            {
                var orderDetails = MapOrderDetails(paymentRequest, paymentModel);

                _paymentRepository.Add(paymentModel);

                _orderRepository.Add(orderDetails);

                int rowsAffected = _paymentRepository.CommitChanges();
            }
            return string.Empty;
        }

        private Order MapOrderDetails(
            MakePaymentRequest paymentRequest,
            PaymentDetail paymentModel)
        {
            var orderDetails = _mapper.Map<Order>(paymentRequest.OrderRequest);

            orderDetails.PaymentId = paymentModel.Id;
            orderDetails.UserId = paymentModel.UserId;
            orderDetails.CreatedDate = paymentModel.CreatedDate;

            orderDetails.OrderItems.ToList().ForEach(x => x.OrderId = orderDetails.Id);

            return orderDetails;
        }
    }
}
