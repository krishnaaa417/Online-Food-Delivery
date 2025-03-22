using ePizza.Domain.Models;
using ePizza.Models.Response;

namespace ePizza.Core.Mappers
{
    public static class UserMappingExtension
    {
        public static IEnumerable<UserResponseModel> ConvertToUserResponseModel(
            this IEnumerable<User> userList)
        {
            List<UserResponseModel> userResponseModelList
                    = new List<UserResponseModel>();

            if (userList.Any())
            {
                foreach (var user in userList)
                {
                    UserResponseModel userResponseModel
                        = new UserResponseModel()
                        {
                            UserId = user.Id,
                            Email = user.Email,
                            Name = user.Name,
                            Password = user.Password,
                            PhoneNumber = user.PhoneNumber,
                            CreatedDate = user.CreatedDate
                        };

                    userResponseModelList.Add(userResponseModel);
                }
            }

            return userResponseModelList;
        }

        public static IEnumerable<UserResponseModel> ConvertToUserResponseModelUsingLinq(
           this IEnumerable<User> userList)
        {
            return userList.Select(x => x.ConvertToUserResponseModelUsingLinq());
        }

        public static UserResponseModel ConvertToUserResponseModelUsingLinq(
                this User user)
        {
            return new UserResponseModel
            {
                UserId = user.Id,
                Email =  user.Email,
                Name = user.Name,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                CreatedDate = user.CreatedDate
            };
        }

     


    }
}
