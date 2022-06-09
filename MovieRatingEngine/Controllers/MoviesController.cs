using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieRatingEngine.Services;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Models;

namespace MovieRatingEngine.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        public MoviesController(IMoviesService movieService)
        {
            _moviesService = movieService;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetMovieDto>>>> Get()
        {
            return Ok(await _moviesService.GetAllMovies());
        }
    }
}
