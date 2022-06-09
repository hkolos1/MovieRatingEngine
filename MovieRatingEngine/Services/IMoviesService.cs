using MovieRatingEngine.Dtos;
using MovieRatingEngine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public interface IMoviesService
    {
        Task<ServiceResponse<List<GetMovieDto>>> GetAllMovies();
        Task<ServiceResponse<GetMovieDto>> GetMovieById(int id);

        Task<ServiceResponse<List<GetMovieDto>>> AddMovie(AddMovieDto newMovie);

        Task<ServiceResponse<GetMovieDto>> UpdateMovie(UpdateMovieDto updatedMovie);

        Task<ServiceResponse<List<GetMovieDto>>> DeleteMovie(int id);





    }
}
