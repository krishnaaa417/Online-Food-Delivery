namespace ePizza.Models.Request
{
    public class DeleteItemFromCartRequest
    {
        public Guid CartId { get; set; }
        public int ItemId { get; set; }
    }
}
