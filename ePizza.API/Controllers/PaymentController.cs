using ePizza.Core.Contracts;
using ePizza.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController: ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public IActionResult MakePayment([FromBody] MakePaymentRequest paymentRequest)
        {

            if (ModelState.IsValid)
            {
                var result = _paymentService.MakePayment(paymentRequest);
                return Ok();
            }

            return BadRequest("Pease check view");
        }
    }
}
