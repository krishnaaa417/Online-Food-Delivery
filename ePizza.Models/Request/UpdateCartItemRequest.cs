namespace ePizza.Models.Request
{
    public class UpdateCartItemRequest
    {
        public Guid CartId { get; set; }

        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
