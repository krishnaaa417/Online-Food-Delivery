using ePizza.Domain.Models;
using ePizza.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ePizza.Repository.Concrete
{
  
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(
            EpizzaHubDbContext dbContext) : base(dbContext)
        {
        }

        public User FindUser(string emailAddress)
        {
            return _dbContext.Users.Include(
                x => x.Roles)
                .Where(x => x.Email == emailAddress).FirstOrDefault();
        }

        public UserToken GetUserToken(int userId)
        {
            return _dbContext.UserTokens.FirstOrDefault(x => x.UserId == userId);
        }

        public bool PersistUserTokens(UserToken userToken)
        {
            var existingToken = _dbContext.UserTokens.FirstOrDefault(x => x.UserId == userToken.UserId);
            if (existingToken != null)
            {
                existingToken.AccessToken = userToken.AccessToken;
                existingToken.RefreshToken = userToken.RefreshToken;
                _dbContext.Entry(existingToken).State= EntityState.Modified;
            }
            else
            {
                _dbContext.UserTokens.Add(userToken);
                
            }
            int rowsAffected = _dbContext.SaveChanges();
            return rowsAffected > 0;
        }
    }
}
