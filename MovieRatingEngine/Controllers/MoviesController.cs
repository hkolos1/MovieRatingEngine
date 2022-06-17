using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRatingEngine.Data;
using MovieRatingEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRatingEngine.Entity;
using MovieRatingEngine.Dtos.Movie;
using MovieRatingEngine.Dtos;

namespace MovieRatingEngine.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IActorMovieService _actorMovieService;
       
        public MoviesController(IMoviesService movieService, IActorMovieService actorMovieService)
        {
            _moviesService = movieService;
            _actorMovieService = actorMovieService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PagingMovie(int? pageNumber, int? pageSize)
        {
            return Ok(await  _moviesService.PagingMovie(pageNumber, pageSize));
        }

        [HttpGet("[action]")]
        public async  Task<IActionResult> SearchMovie(string searchBar, Category type=Category.Movie)
        {
            return Ok(await _moviesService.SearchMovie(searchBar, type));
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> ProuzrokujServerError()
        {
            return Ok(await _moviesService.TestErrorMiddleware());
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = nameof(Role.Admin) + ", " + nameof(Role.User))]
        public async Task<ActionResult<ServiceResponse<List<GetMovieDto>>>> Get([FromQuery]PaginationNumbers request)
        {
            
            return Ok(await _moviesService.GetAllMovies(request));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = nameof(Role.Admin) + ", " + nameof(Role.User))]
        public async Task<ActionResult<ServiceResponse<GetMovieDto>>> GetSingle(Guid id)
        {
            return Ok(await _moviesService.GetMovieById(id));
        }

        [HttpPost]

        public async Task<ActionResult<ServiceResponse<List<GetMovieDto>>>> AddMovie([FromForm] AddMovieDto newMovie)

        {
            return Ok(await _moviesService.AddMovie(newMovie));
        }

        [HttpPut]

        public async Task<ActionResult<ServiceResponse<List<UpdateMovieDto>>>> UpdateMovie([FromForm] UpdateMovieDto updateMovie)

        {
            var response = await _moviesService.UpdateMovie(updateMovie);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult<ServiceResponse<List<GetMovieDto>>>> DeleteMovie(Guid id)
        {
            var response = await _moviesService.DeleteMovie(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("{movieId}/deleteactor/{actorId}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> DeleteMovie(Guid movieId, Guid actorId)
        {
            var response = await _actorMovieService.DeleteActorFromMovie(actorId, movieId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok();
        }
        [HttpPost("{movieId}/addactor")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> AddActorToMovie(Guid movieId, AddActorsToMovie request)
        {
            return Ok(await _actorMovieService.AddActortToMovie(movieId, request));
        }
    }
}
