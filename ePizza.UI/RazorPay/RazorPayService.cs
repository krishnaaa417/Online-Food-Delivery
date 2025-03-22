using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace ePizza.UI.RazorPay
{
    public class RazorPayService : IRazorPayService
    {
        private readonly RazorpayClient _razorpayClient;
        private readonly IConfiguration _configuration;

        public RazorPayService(IConfiguration configuration)
        {
            _configuration = configuration;
            _razorpayClient = new RazorpayClient(_configuration["RazorPay:Key"], _configuration["RazorPay:Secret"]);
        }


        public string CreateOrder(decimal amount, string currency, string receipt)
        {
            Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "amount", amount },
                { "receipt", receipt },
                { "currency", currency }
            };
            Order order = _razorpayClient.Order.Create(options);
            return order["id"].ToString();
        }

        public Payment GetPayment(string paymentId)
        {
            return _razorpayClient.Payment.Fetch(paymentId);
        }

        public bool VerifySignature(string signature, string orderId, string paymentId)
        {
            string payload = string.Format("{0}|{1}", orderId, paymentId);
            string secret = RazorpayClient.Secret;
            string actualSignature = getActualSignature(payload, secret);
            return actualSignature.Equals(signature);
        }

        private static string getActualSignature(string payload, string secret)
        {
            byte[] secretBytes = StringEncode(secret);
            HMACSHA256 hashHmac = new HMACSHA256(secretBytes);
            var bytes = StringEncode(payload);

            return HashEncode(hashHmac.ComputeHash(bytes));
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }

        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
