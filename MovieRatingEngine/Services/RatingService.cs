using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.Data;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Movie;
using MovieRatingEngine.Dtos.Rating;
using MovieRatingEngine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public class RatingService : IRatingService
    {
        private readonly MovieContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMoviesService _moviesService;
        private static List<int> acceptedRating;

        public RatingService(MovieContext db, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMoviesService moviesService)
        {
            _db = db ??
                throw new ArgumentNullException(nameof(db));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
            acceptedRating = new List<int>() { 1, 2, 3, 4, 5 };
            _moviesService = moviesService ??
                throw new ArgumentNullException(nameof(_moviesService));
        }

        private Guid GetUserId() => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        private async Task<string> AddRating(AddRatingDto request)
        {
            if (!acceptedRating.Contains(request.YourRating))
                return "Rating format is not accepted. It should be in range of 1 to 5";
            try
            {
                var rating = _mapper.Map<Entity.Rating>(request);
                rating.UserId = GetUserId();
                rating.CreatedAt = DateTime.Now;

                await _db.AddAsync(rating);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return null;

        }

        //main method to save rating 
        public async Task<ServiceResponse<GetRatigDto>> StoreRating(AddRatingDto request)
        {
            var response = new ServiceResponse<GetRatigDto>();

            try
            {
                //check if requsted movie exists
                var movie = await _db.Movies.FirstOrDefaultAsync(x => x.Id == request.MovieId) ?? throw new Exception("Movie not found.");

                string exceptionString = null;
                //check if it's request for create or update
                var rating = await CheckIfMovieUserRatingExists(request.MovieId, GetUserId());
                if (rating != null)
                {
                    exceptionString = await UpdateRatingMethod(rating, request);
                    if (exceptionString != null)
                        throw new Exception(exceptionString);
                    response.Message = "Updated";
                }
                else
                {
                    exceptionString = AddRating(request).Result;
                    if (exceptionString != null)
                        throw new Exception(exceptionString);
                    response.Message = "Created";

                }

                //change averageRating in Movie // i don't think it's necessary to store this in table, it can be calculated for dto 
                exceptionString = await _moviesService.SetRating(movie);
                if (exceptionString != null)
                    throw new Exception(exceptionString);

                //get saved rating from db
                Rating dbRating = await GetRatingFromDb(request.MovieId, GetUserId());
                response.Data = _mapper.Map<GetRatigDto>(dbRating);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;


        }

        private async Task<Rating> CheckIfMovieUserRatingExists(Guid movieId, Guid userId)
        {
            return await _db.Ratings.Where(x => x.MovieId == movieId && x.UserId == userId).FirstOrDefaultAsync();
        }

        private async Task<Rating> GetRatingFromDb(Guid movieId, Guid userId)
        {
            return await _db.Ratings.Include(x => x.User).Include(x => x.Movie).FirstOrDefaultAsync(x => x.MovieId == movieId && x.UserId == userId);
        }

        public async Task<ServiceResponse<List<GetRatigDto>>> GetRatingsByMovieId(Guid movieId)
        {
            var response = new ServiceResponse<List<GetRatigDto>>();
            try
            {
                //var movie = await _db.Movies.Include(x => x.Ratings).FirstOrDefaultAsync(x => x.Id == movieId) ?? throw new Exception("Movie not found.");

                //response.Data = _mapper.Map<List<GetRatigDto>>(movie.Ratings.OrderByDescending(x => x.CreatedAt).Select(x => _mapper.Map<GetRatigDto>(x)));
                var ratings = await _db.Ratings.Where(x=>x.MovieId==movieId).Include(x=>x.User).Include(x=>x.Movie)
                    .OrderByDescending(x=>x.CreatedAt).ToListAsync()?? throw new Exception("Movie not found.");

                response.Data = _mapper.Map<List<GetRatigDto>>(ratings.Select(x => _mapper.Map<GetRatigDto>(x)));
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }


            return response;
        }

        public async Task<ServiceResponse<List<GetRatigDto>>> GetRatingsByUserId(Guid userId)
        {
            var response = new ServiceResponse<List<GetRatigDto>>();
            try
            {
                var userRatings = await _db.Users.Include(x => x.Ratings).FirstOrDefaultAsync(x => x.Id == userId);

                response.Data = _mapper.Map<List<GetRatigDto>>(userRatings.Ratings.OrderByDescending(x => x.CreatedAt).Select(x => _mapper.Map<GetRatigDto>(x)));
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }


            return response;
        }
        public async Task<ServiceResponse<List<GetRatigDto>>> GetRatingsOfLoggedUser()
        {
            var response = new ServiceResponse<List<GetRatigDto>>();
            try
            {

                var userRatings = await _db.Users.Include(x => x.Ratings).FirstOrDefaultAsync(x => x.Id == GetUserId());

                response.Data = _mapper.Map<List<GetRatigDto>>(userRatings.Ratings.OrderByDescending(x => x.CreatedAt).Select(x => _mapper.Map<GetRatigDto>(x)));
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }


            return response;
        }
        public async Task<ServiceResponse<GetRatigDto>> GetSingleRatingByRatingId(Guid movieId)
        {
            var response = new ServiceResponse<GetRatigDto>();

            try
            {
                var rating = await GetRatingFromDb(movieId, GetUserId()) ?? throw new Exception("Rating not found.");
                response.Data = _mapper.Map<GetRatigDto>(rating);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;
        }

        private async Task<string> UpdateRatingMethod(Rating rating, AddRatingDto request)
        {
            if (!acceptedRating.Contains(request.YourRating))
                return "Rating format is not accepted. It should be in range of 1 to 5";
            try
            {
                rating.YourRating = request.YourRating;
                rating.UserId = GetUserId();
                rating.CreatedAt = DateTime.Now;
                await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }


            return null;
        }

        public async Task<ServiceResponse<bool>> DeleteRating(Guid movieId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var rating = await CheckIfMovieUserRatingExists(movieId, GetUserId());
                if (rating != null)
                {
                    _db.Remove(rating);
                    await _db.SaveChangesAsync();
                    var movie = await _db.Movies.Include(x=>x.Ratings).FirstOrDefaultAsync(x => x.Id == movieId);
                    var exSetRating = await _moviesService.SetRating(movie);
                    if (exSetRating != null)
                        throw new Exception(exSetRating);
                     response.Message = "Deleted";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            };
            return response;
        }
    }
}
