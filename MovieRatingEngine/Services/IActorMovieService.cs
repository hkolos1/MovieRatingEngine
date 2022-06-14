using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Actor;
using MovieRatingEngine.Dtos.Movie;
using MovieRatingEngine.Entity;
using System;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public interface IActorMovieService
    {
        Task<string> AddActorToMovie(Guid actorId, Movie movie);
        Task<string> AddNewActorToMovie(AddActorDto actorDto, Movie movie);
        Task<ServiceResponse<bool>> DeleteActorFromMovie(Guid actorId, Guid movieId);
        Task<ServiceResponse<GetMovieDto>> AddActortToMovie(Guid movieId, AddActorsToMovie request);
    }
}
