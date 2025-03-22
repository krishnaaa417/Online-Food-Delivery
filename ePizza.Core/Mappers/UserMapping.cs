using AutoMapper;
using ePizza.Domain.Models;
using ePizza.Models.Request;
using ePizza.Models.Response;

namespace ePizza.Core.Mappers
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserResponseModel>()
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.Id))
                .ReverseMap();

            CreateMap<CreateUserRequest, User>();

            CreateMap<Item, ItemResponseModel>();

            CreateMap<UserToken, UserTokenModel>().ReverseMap();
        }
    }
}
