using AutoMapper;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public class MoviesService : IMoviesService
    {
        private static List<Movie> movies = new List<Movie> {
           new Movie(),
        };
        private readonly IMapper _mapper;

        public MoviesService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetMovieDto>>> GetAllMovies()
        {
            var serviceResponse = new ServiceResponse<List<GetMovieDto>>();
            serviceResponse.Data = movies.Select(c => _mapper.Map<GetMovieDto>(c)).ToList();
            return await Task.FromResult(serviceResponse);
        }
    }

}
