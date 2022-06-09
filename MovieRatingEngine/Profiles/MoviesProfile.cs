using AutoMapper;

namespace MovieRatingEngine.Profiles
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<Models.Movie, Dtos.GetMovieDto>();

            CreateMap<Models.Movie, Dtos.GetMovieDto>().ReverseMap();

        }
    }
}
