using AutoMapper;

namespace MovieRatingEngine.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Models.User, Dtos.User.UserAddDto>();
            CreateMap<Models.User, Dtos.User.UserLoginDto>();

            CreateMap<Models.User, Dtos.User.UserAddDto>().ReverseMap();
            CreateMap<Models.User, Dtos.User.UserLoginDto>().ReverseMap();

        }
    }
}
