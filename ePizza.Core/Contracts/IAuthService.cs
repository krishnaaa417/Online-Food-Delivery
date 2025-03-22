using ePizza.Domain.Models;
using ePizza.Models.Response;

namespace ePizza.Core.Contracts
{
    public interface IAuthService
    {

        ValidateUserResponse ValidateUser(string username, string password);

        UserResponseModel GetUserDetails(string userName);

        bool PersistUserToken(UserTokenModel userTokenModel);

        UserTokenModel GetSavedTokenDetail(string userName);
    }
}
