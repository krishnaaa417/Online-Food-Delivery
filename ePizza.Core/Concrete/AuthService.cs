using AutoMapper;
using ePizza.Core.Contracts;
using ePizza.Core.CustomExceptions;
using ePizza.Domain.Models;
using ePizza.Models.Response;
using ePizza.Repository.Contracts;
using System.Security.Authentication;

namespace ePizza.Core.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            this._userRepository = userRepository;
            _mapper = mapper;
        }

        public UserTokenModel GetSavedTokenDetail(string userName)
        {
            var user = GetUserDetails(userName);

            var userToken = _userRepository.GetUserToken(user.UserId);

            return _mapper.Map<UserTokenModel>(userToken);
        }

        public UserResponseModel GetUserDetails(string userName)
        {
            var userDetails = _userRepository.FindUser(userName);

            if (userDetails == null)
                throw new RecordNotFoundException($"No user found in database against User with email as {userName}");

            return _mapper.Map<UserResponseModel>(userDetails);
        }

        public bool PersistUserToken(UserTokenModel userTokenModel)
        {
            var token =  _mapper.Map<UserToken>(userTokenModel);

            return _userRepository.PersistUserTokens(token);
        }

        public ValidateUserResponse ValidateUser(string username, string password)
        {
            var userDetails = _userRepository.FindUser(username); // if user exists

            if (userDetails != null)
            {
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, userDetails.Password);

                if (!isValidPassword)
                    throw new InvalidCredentialException($"Invalid credentials passed for the user {username}");

                return new ValidateUserResponse()
                {
                    Email = username,
                    Name = userDetails.Name,
                    UserId = userDetails.Id,
                    Roles = userDetails.Roles.Select(x => x.Name).ToList(),
                };
            }
            throw new Exception($"The user with email address {username} doesn't exists.");
        }
    }
}