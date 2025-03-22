namespace ePizza.Models.Response
{
    public class UserTokenModel
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
