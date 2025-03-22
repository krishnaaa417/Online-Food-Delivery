namespace ePizza.UI.Helpers.TokenHelpers
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public string GetToken()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                return _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];
            }
            throw new Exception("Access token could not be found.");
        }

        public void SetToken(string token)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append(
                    "AccessToken", token,
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(60),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
            }
        }
    }
}
