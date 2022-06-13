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
using Microsoft.AspNetCore.Authorization;
using MovieRatingEngine.Data;

namespace MovieRatingEngine.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private MovieContext _movieContext;
        public MoviesController(IMoviesService movieService, MovieContext movieContext)
        {
            _moviesService = movieService;
            _movieContext = movieContext;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PagingMovie(int? pageNumber, int? pageSize)
        {
            return Ok(await  _moviesService.PagingMovie(pageNumber, pageSize));
        }

        [HttpGet("[action]")]
        public async  Task<IActionResult> SearchMovie(string Title, string ReleaseDate)
        {
            return Ok(await _moviesService.SearchMovie(Title, ReleaseDate));
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

        public async Task<ActionResult<ServiceResponse<List<GetMovieDto>>>> AddMovie([FromForm]AddMovieDto newMovie)

        {
            return Ok(await _moviesService.AddMovie(newMovie));
        }

        [HttpPut]

        public async Task<ActionResult<ServiceResponse<List<UpdateMovieDto>>>> UpdateMovie([FromForm]UpdateMovieDto updateMovie)

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
    }
}
