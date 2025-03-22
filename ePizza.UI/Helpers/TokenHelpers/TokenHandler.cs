using System.Net.Http.Headers;

namespace ePizza.UI.Helpers.TokenHelpers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public TokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var token = _tokenService.GetToken();

            // If the token is available, add it to the Authorization header
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return base.SendAsync(request, cancellationToken);
        }

    }
}
