using ePizza.Domain.Models;

namespace ePizza.Repository.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {

        User FindUser(string emailAddress);

        bool PersistUserTokens(UserToken userToken);

        UserToken GetUserToken(int userId);
    }
}
