using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRatingEngine.Data;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRatingEngine.Entity;

namespace MovieRatingEngine.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IActorMovieService _actorMovieService;
        private MovieContext _movieContext;
        public MoviesController(IMoviesService movieService, MovieContext movieContext, IActorMovieService actorMovieService)
        {
            _moviesService = movieService;
            _movieContext = movieContext;
            _actorMovieService = actorMovieService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PagingMovie(int? pageNumber, int? pageSize)
        {
            return Ok(await  _moviesService.PagingMovie(pageNumber, pageSize));
        }

        [HttpGet("[action]")]
        public async  Task<IActionResult> SearchMovie(string searchBar, Category type)
        {
            return Ok(await _moviesService.SearchMovie(searchBar, type));
        }


        [HttpGet("GetAll")]
        [Authorize(Roles = nameof(Role.Admin) + ", " + nameof(Role.User))]
        public async Task<ActionResult<ServiceResponse<List<GetMovieDto>>>> Get()
        {
            return Ok(await _moviesService.GetAllMovies());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = nameof(Role.Admin) + ", " + nameof(Role.User))]
        public async Task<ActionResult<ServiceResponse<GetMovieDto>>> GetSingle(Guid id)
        {
            return Ok(await _moviesService.GetMovieById(id));
        }

        [HttpPost]

        public async Task<ActionResult<ServiceResponse<List<GetMovieDto>>>> AddMovie([FromBody] AddMovieDto newMovie)//, [FromBody]List<AddActorDto> addNewActors)

        {
            return Ok(await _moviesService.AddMovie(newMovie));//, addNewActors));
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
        [HttpDelete("deleteactor/{movieId}/{actorId}")]
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
        [HttpPost("{movieId}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> AddActorToMovie(Guid movieId, AddActorsToMovie request)
        {
            return Ok(await _actorMovieService.AddActortToMovie(movieId, request));
        }
    }
}
