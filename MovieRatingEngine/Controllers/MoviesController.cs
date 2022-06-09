﻿using System;
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

namespace MovieRatingEngine.Controllers
{
    [Authorize]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetMovieDto>>> GetSingle(int id)
        {
            return Ok(await _moviesService.GetMovieById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetMovieDto>>>> AddCharacter(AddMovieDto newMovie)
        {
            return Ok(await _moviesService.AddMovie(newMovie));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<UpdateMovieDto>>>> UpdateCharacter(UpdateMovieDto updateMovie)
        {
            var response = await _moviesService.UpdateMovie(updateMovie);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetMovieDto>>>> Delete(int id)
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
