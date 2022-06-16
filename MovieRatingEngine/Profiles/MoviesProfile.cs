using AutoMapper;
using MovieRatingEngine.Dtos.Movie;

namespace MovieRatingEngine.Profiles
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<Entity.Movie, GetMovieDto>();
            CreateMap<Entity.Movie, AddMovieDto>();
            CreateMap<Entity.Movie, UpdateMovieDto>();

            CreateMap<Entity.Movie, GetMovieDto>().ReverseMap();
            CreateMap<Entity.Movie, AddMovieDto>().ReverseMap();
            CreateMap<Entity.Movie, UpdateMovieDto>().ReverseMap();

        }
    }
}
