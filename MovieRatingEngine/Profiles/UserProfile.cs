using AutoMapper;

namespace MovieRatingEngine.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entity.User, Dtos.User.UserAddDto>();
            CreateMap<Entity.User, Dtos.User.UserLoginDto>();

            CreateMap<Entity.User, Dtos.User.UserAddDto>().ReverseMap();
            CreateMap<Entity.User, Dtos.User.UserLoginDto>().ReverseMap();

        }
    }
}
