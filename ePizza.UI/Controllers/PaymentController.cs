using ePizza.UI.Helpers;
using ePizza.UI.Models.ApiResponses;
using ePizza.UI.Models;
using Microsoft.AspNetCore.Mvc;
using ePizza.UI.RazorPay;
using ePizza.UI.Models.ApiRequest;
using ePizza.UI.Models.ViewModels;

namespace ePizza.UI.Controllers
{
    public class PaymentController : BaseController
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRazorPayService _razorPayService;

        public PaymentController(IConfiguration configuration,
          IHttpClientFactory httpClientFactory,
          IRazorPayService razorPayService)
        {
            _httpClientFactory = httpClientFactory;
            _razorPayService = razorPayService;
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            PaymentModel payment = new PaymentModel();
            GetCartResponseModel cart = TempData.Peek<GetCartResponseModel>("CartDetails");
            if (cart != null)
            {
                payment.RazorpayKey = _configuration["RazorPay:Key"];
                payment.Cart = cart;
                payment.GrandTotal = Math.Round(cart.GrantTotal);
                payment.Currency = "INR";
                payment.Description = string.Join(",", cart.Items.Select(r => r.ItemName));
                payment.Receipt = Guid.NewGuid().ToString();

                payment.OrderId = _razorPayService.CreateOrder(payment.GrandTotal * 100, payment.Currency, payment.Receipt);


                return View(payment);
            }
            return RedirectToAction("Index", "Cart");
        }

        public async Task<IActionResult> Status(IFormCollection form)
        {
            if (form.Keys.Count > 0)
            {
                string paymentId = form["rzp_paymentid"];
                string orderId = form["rzp_orderid"];
                string signature = form["rzp_signature"];
                string transactionId = form["Receipt"];
                string currency = form["Currency"];

                bool isSignatureValid = _razorPayService.VerifySignature(signature, orderId, paymentId);

                if (isSignatureValid)
                {
                    var payment = _razorPayService.GetPayment(paymentId);
                    string status = payment["status"];


                    var paymentRequestModel = GetPaymentRequest(paymentId,orderId,transactionId,currency,status);

                    using var httpClient = _httpClientFactory.CreateClient("ePizaaApiClient");

                    var paymentRequest = await httpClient.PostAsJsonAsync($"api/Payment", paymentRequestModel);

                    paymentRequest.EnsureSuccessStatusCode();

                    Response.Cookies.Delete("CartId");
                    TempData.Remove("CartDetails");
                    TempData.Remove("Address");
                    
                    
                    TempData.Set("PaymentDetails", paymentRequestModel);
                    return RedirectToAction("Receipt");
                }
            }
            ViewBag.Message = "Payment Failed";
            return View();
        }


        public IActionResult Receipt()
        {
            MakePaymentRequestModel model = TempData.Peek<MakePaymentRequestModel>("PaymentDetails");
            if (model != null)
            {
                var receiptViewModel
                    = new ReceiptViewModel()
                    {
                        GrandTotal = model.GrandTotal,
                        Currency = model.Currency,
                        CreatedDate = model.CreatedDate,
                        Email = model.Email,
                        UserId = model.UserId,
                        CartId = model.CartId,
                        Status = model.Status,
                        Total = model.Total,
                        Tax = model.Tax,
                        TransactionId = model.TransactionId,
                        Id = model.Id
                    };
                TempData.Remove("PaymentDetails");
                
                return View(receiptViewModel);
            }
            return View();
        }

        private MakePaymentRequestModel GetPaymentRequest(
            string paymentId, 
            string orderId, 
            string transactionId,
            string currency, 
            string status)
        {
            GetCartResponseModel cart = TempData.Peek<GetCartResponseModel>("CartDetails");
            AddressViewModel addressViewModel = TempData.Peek<AddressViewModel>("Address");

            return new MakePaymentRequestModel()
            {
                CartId = cart.Id,
                Total = cart.Total,
                Tax = cart.Tax,
                GrandTotal = cart.GrantTotal,
                Currency = currency,
                CreatedDate = DateTime.UtcNow,
                Status = status,
                Email = CurrentUser.Email,
                UserId = CurrentUser.UserId,
                Id = paymentId, 
                TransactionId = transactionId,
                OrderRequest = new OrderRequest()
                {
                    Id = orderId,
                    Street = addressViewModel.Street,
                    City = addressViewModel.City,
                    Locality = addressViewModel.Locality,
                    ZipCode = addressViewModel.ZipCode,
                    UserId = CurrentUser.UserId,
                    PhoneNumber = addressViewModel.PhoneNumber,
                    OrderItems = GetOrderItems(cart.Items)
                }
            };
        }

        private List<OrderItems> GetOrderItems(List<CartItemResponse> cartItems)
        {
            List<OrderItems> orderItems = new();

            foreach (var item in cartItems)
            {
                OrderItems items = new()
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Total = item.ItemTotal
                };

                orderItems.Add(items);
            }

            return orderItems;
        }
    }

}
