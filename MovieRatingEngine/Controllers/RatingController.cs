using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Rating;
using MovieRatingEngine.Models;
using MovieRatingEngine.Services;
using System;
using System.Threading.Tasks;

namespace MovieRatingEngine.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService??
                throw new ArgumentNullException(nameof(ratingService));

        }

        //for creating and updating
        [Authorize(Roles = nameof(Role.User))]
        [HttpPost("create-rating")]
        public async Task<IActionResult> AddRating(AddRatingDto request)
        {
            var response = await _ratingService.StoreRating(request);
            if (response.Success == false)
                return BadRequest();
            return StatusCode(StatusCodes.Status201Created,response );
        }
        
        [Authorize(Roles = nameof(Role.User))]
        [HttpPut("update-rating")]
        public async Task<IActionResult> UpdateRating(AddRatingDto request)
        {
            var response = await _ratingService.StoreRating(request);
            if (response.Success == false)
                return BadRequest();
            return StatusCode(StatusCodes.Status200OK, await _ratingService.StoreRating(request));
        }
        
        [HttpGet("movie-ratings/{movieId}")]
        public async Task<IActionResult> GetMovieRatings(Guid movieId)
        {
            return Ok(await _ratingService.GetRatingsByMovieId(movieId));
        }
        [HttpGet("rating-of-user/{userId}")]
        public async Task<IActionResult> GetRatingsByUserId(Guid userId)
        {
            return Ok(await _ratingService.GetRatingsByUserId(userId));
        }

        [Authorize(Roles = nameof(Role.User))]
        [HttpGet("user-ratings")]
        public async Task<IActionResult> GetRatingsByUserId()
        {
            return Ok(await _ratingService.GetRatingsOfLoggedUser());
        }
        [Authorize(Roles = nameof(Role.User))]
        [HttpGet("user-movie-rating")]
        public async Task<IActionResult> GetUserMovieRating(Guid movieId)
        {
            return Ok(await _ratingService.GetSingleRatingByRatingId(movieId));
        }

        [Authorize(Roles =nameof(Role.User))]
        [HttpDelete]
        public async Task<IActionResult> DeleteRating(Guid movieId)
        {
            var response=await _ratingService.DeleteRating(movieId);
            if (response.Success == false)
                return BadRequest(response);
            else
                return Ok(response);
        }
    }
}
