using ePizza.Core.Utils;
using ePizza.Core.Contracts;
using ePizza.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ePizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly TokenGenerator _tokenGenerator;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService,
            TokenGenerator tokenGenerator,
            ILogger<AuthController> logger,
            IConfiguration configuration)
        {
            _authService = authService;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateUser(string userName, string password)
        {
            var userDetails = _authService.ValidateUser(userName, password);

            if (userDetails.UserId > 0)
            {
                _logger.LogInformation($"The current passed in username is {userName}");

                var securityToken = _tokenGenerator.GenerateToken(userDetails);

                var authApiResponse
                     = new AuthApiResponse()
                     {
                         AccessToken = securityToken,
                         TokenExpiryInSeconds = Convert.ToInt32(_configuration["Jwt:TokenExpiryInMinutes"]),
                         RefreshToken = _tokenGenerator.GenerateRefreshToken()
                     };

                PersistUserToken(userDetails, authApiResponse);

                _logger.LogInformation($"Token generated successfully");

                return Ok(authApiResponse);
            }

            return BadRequest("User Resonse Is not valid");
        }


        [HttpPost("token-refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenRequest tokenModel)
        {
            var principal = _tokenGenerator.GetTokenPrincipal(tokenModel.AccessToken);

            if (principal == null) return Unauthorized("Invalid access token");

            var username = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var rolesClaim = principal.Claims.FirstOrDefault(x => x.Type == "Roles").Value;

            List<string> roles = rolesClaim != null
                ? JsonSerializer.Deserialize<List<string>>(rolesClaim)
                : new List<string>();

            var previousTokenDetails = _authService.GetSavedTokenDetail(username);

            if (previousTokenDetails == null
                || previousTokenDetails.RefreshToken != tokenModel.RefreshToken
                || previousTokenDetails.RefreshTokenExpiryTime < DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            var userDetails = _authService.GetUserDetails(username);
            userDetails.Roles = roles;

            var newAccessToken = _tokenGenerator.GenerateToken(GetUserResponseObject(userDetails));
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

            previousTokenDetails.RefreshToken = newRefreshToken;
            previousTokenDetails.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _authService.PersistUserToken(previousTokenDetails);
            // udpate token in my database

            return Ok(new AuthApiResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                TokenExpiryInSeconds = Convert.ToInt32(_configuration["Jwt:TokenExpiryInMinutes"]),
            });
        }

        private void PersistUserToken(
          ValidateUserResponse userDetails,
          AuthApiResponse authApiResponse)
        {
            _authService.PersistUserToken(new UserTokenModel()
            {
                AccessToken = authApiResponse.AccessToken,
                RefreshToken = authApiResponse.RefreshToken,
                UserId = userDetails.UserId,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
            });
        }
        private ValidateUserResponse GetUserResponseObject(UserResponseModel userResponse)
        {
            return new ValidateUserResponse
            {
                Email = userResponse.Email,
                Name = userResponse.Name,
                Roles = userResponse.Roles,
                UserId = userResponse.UserId
            };
        }
    }
}
