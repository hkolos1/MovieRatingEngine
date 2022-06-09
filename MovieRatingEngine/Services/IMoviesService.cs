using MovieRatingEngine.Dtos;
using MovieRatingEngine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public interface IMoviesService
    {
        Task<ServiceResponse<List<GetMovieDto>>> GetAllMovies();

    }
}
