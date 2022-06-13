using AutoMapper;

namespace MovieRatingEngine.Profiles
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<Entity.Movie, Dtos.GetMovieDto>();
            CreateMap<Entity.Movie, Dtos.AddMovieDto>();
            CreateMap<Entity.Movie, Dtos.UpdateMovieDto>();

            CreateMap<Entity.Movie, Dtos.GetMovieDto>().ReverseMap();
            CreateMap<Entity.Movie, Dtos.AddMovieDto>().ReverseMap();
            CreateMap<Entity.Movie, Dtos.UpdateMovieDto>().ReverseMap();

        }
    }
}
