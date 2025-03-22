namespace ePizza.Models.Request
{
    public class UpdateCartUserRequest
    {
        public Guid CartId { get; set; }
        public int UserId { get; set; }
    }
}
