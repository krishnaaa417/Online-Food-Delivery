namespace ePizza.Models.Request
{
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
