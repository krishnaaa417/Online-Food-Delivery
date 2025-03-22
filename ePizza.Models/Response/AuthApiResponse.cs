namespace ePizza.Models.Response
{
    public class AuthApiResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int TokenExpiryInSeconds { get; set; }   
    }


    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

    }
}
