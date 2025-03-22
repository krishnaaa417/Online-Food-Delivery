using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ePizza.UI.Models.ApiResponses;
using ePizza.UI.Models.ViewModels;
using ePizza.UI.Helpers.TokenHelpers;

namespace ePizza.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService _tokenService;

        public LoginController(
            IHttpClientFactory httpClientFactory,
            ITokenService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("ePizaaApiClient");

                var userResponse = await client.GetFromJsonAsync<ApiResponseModel<ValidateUserResponseModel>>(
                    $"api/Auth?userName={loginModel.UserName}&password={loginModel.Password}");

                if (userResponse.Success)
                {
                    var accessToken = userResponse.Data.AccessToken;  // access token 

                    _tokenService.SetToken(accessToken);

                    var tokenHandler = new JwtSecurityTokenHandler();
       
                    var tokenDetails = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

                    List<Claim> claims =
                        new List<Claim>();

                    foreach (var item in tokenDetails.Claims)
                    {
                        claims.Add(new Claim(item.Type,item.Value));
                    }
                    await GenerateTicket(claims);


                    bool isAdmin = Convert.ToBoolean(claims.Where(x => x.Type == "IsAdmin").FirstOrDefault().Value);

                    if (isAdmin)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "User" });
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Invalid Credentials");
                }
                // do relevant functionality
            }

            return View();
        }

        private async Task GenerateTicket(List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties()
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                });
        }


        [HttpGet]
        [Authorize]
        public IActionResult WelcomeScreen()
        {
            return View();
        }


        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult UnAuthorize()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // TODO : remove token from cookies, sessionstorage
            return RedirectToAction("Login", "Login");
        }
    }
}