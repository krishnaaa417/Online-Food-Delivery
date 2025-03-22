
namespace ePizza.UI.Helpers.TokenHelpers
{
    public interface ITokenService
    {
        void SetToken(string token);  // save token in cookies(localstorage, sessionstorage)

        string GetToken();  // get token from cookies
    }
}
