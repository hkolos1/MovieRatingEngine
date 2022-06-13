using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Actor;
using MovieRatingEngine.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public interface IMoviesService
    {
        Task<ServiceResponse<List<GetMovieDto>>> GetAllMovies();
        Task<ServiceResponse<GetMovieDto>> GetMovieById(Guid id);

        Task<ServiceResponse<List<GetMovieDto>>> AddMovie(AddMovieDto newMovie);//, List<AddActorDto> addNewActors);

        Task<ServiceResponse<GetMovieDto>> UpdateMovie(UpdateMovieDto updatedMovie);

        Task<ServiceResponse<List<GetMovieDto>>> DeleteMovie(Guid id);
      
        Task<string> SetRating(Movie movie, int yourRating);
    }
}
