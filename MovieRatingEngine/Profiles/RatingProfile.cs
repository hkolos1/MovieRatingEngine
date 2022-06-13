using AutoMapper;
using MovieRatingEngine.Dtos.Rating;
using MovieRatingEngine.Dtos.User;
using MovieRatingEngine.Entity;

namespace MovieRatingEngine.Profiles
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating, GetRatigDto>()
                .ForMember(x=> x.Username, opt=>opt.MapFrom(o=>o.User.Username))
                .ForMember(x=>x.Title, opt=>opt.MapFrom(o=>o.Movie.Title));
           
            CreateMap<Rating, AddRatingDto>();

            CreateMap<Rating, AddRatingDto>().ReverseMap();
            CreateMap<Rating, GetRatigDto>().ReverseMap();
        }
    }
}
