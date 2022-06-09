using AutoMapper;

namespace MovieRatingEngine.Profiles
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<Models.Movie, Dtos.GetMovieDto>();
            CreateMap<Models.Movie, Dtos.AddMovieDto>();
            CreateMap<Models.Movie, Dtos.UpdateMovieDto>();

            CreateMap<Models.Movie, Dtos.GetMovieDto>().ReverseMap();
            CreateMap<Models.Movie, Dtos.AddMovieDto>().ReverseMap();
            CreateMap<Models.Movie, Dtos.UpdateMovieDto>().ReverseMap();

        }
    }
}
