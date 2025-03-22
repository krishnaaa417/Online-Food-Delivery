using AutoMapper;
using ePizza.Core.Contracts;
using ePizza.Core.Mappers;
using ePizza.Domain.Models;
using ePizza.Models.Request;
using ePizza.Models.Response;
using ePizza.Repository.Contracts;

namespace ePizza.Core.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IRolesRepository rolesRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
            _mapper = mapper;
        }

        public bool AddUser(CreateUserRequest userRequest)
        {
            // TODO : Refractor
            var roles = _rolesRepository.GetAll().Where(x => x.Name == "User").FirstOrDefault();

            if (roles != null)
            {
                var userDetails = _mapper.Map<User>(userRequest);

                userDetails.Roles.Add(roles);

                userDetails.Password = BCrypt.Net.BCrypt.HashPassword(userDetails.Password);

                _userRepository.Add(userDetails);

                int rowsAffected = _userRepository.CommitChanges();

                return rowsAffected > 0;
            }

            return false;
        }

        public IEnumerable<UserResponseModel> GetAllUsers()
        {
            var users = _userRepository.GetAll().AsEnumerable();

            var userResponse = _mapper.Map<IEnumerable<UserResponseModel>>(users);

            return userResponse;

            //return users.ConvertToUserResponseModelUsingLinq();
        }

        public UserResponseModel GetUserById(int userId)
        {
            var users = _userRepository.GetAll().AsEnumerable().FirstOrDefault();

            return users!.ConvertToUserResponseModelUsingLinq();
        }
    }
}
