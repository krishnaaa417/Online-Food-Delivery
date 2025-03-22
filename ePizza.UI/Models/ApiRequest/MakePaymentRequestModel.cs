using System.ComponentModel.DataAnnotations;

namespace ePizza.UI.Models.ApiRequest
{
    public class MakePaymentRequestModel
    {
        public string Id { get; set; }

        public string TransactionId { get; set; } // will come from Razor Pay

        public decimal Tax { get; set; }

        public string Currency { get; set; }

        public decimal Total { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Status { get; set; }

        public Guid CartId { get; set; }

        public decimal GrandTotal { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }

        public OrderRequest OrderRequest { get; set; }

    }
    public class OrderRequest
    {
        public OrderRequest()
        {
            OrderItems = new List<OrderItems>();
        }

        public string Id { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Locality { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public int UserId { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }
    }

    public class OrderItems
    {
        public int ItemId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Total { get; set; }
    }
}
