using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Movie;
using MovieRatingEngine.Dtos.Rating;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public interface IRatingService
    {
        Task<ServiceResponse<GetRatigDto>> GetSingleRatingByRatingId(Guid movieId);
        Task<ServiceResponse<List<GetRatigDto>>> GetRatingsByUserId(Guid userId);
        Task<ServiceResponse<List<GetRatigDto>>> GetRatingsOfLoggedUser();
        Task<ServiceResponse<List<GetRatigDto>>> GetRatingsByMovieId(Guid movieId);
        Task<ServiceResponse<GetRatigDto>> StoreRating(AddRatingDto request);
        Task<ServiceResponse<bool>> DeleteRating(Guid movieId);
    }
}
