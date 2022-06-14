using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.Data;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Actor;
using MovieRatingEngine.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public class ActorMovieService : IActorMovieService
    {
        private readonly MovieContext _db;
        private readonly IActorService _actorService;


        public ActorMovieService(MovieContext db, IActorService actorService)
        {
            _db = db;
            _actorService = actorService;
        }

        public async Task<string> AddActorToMovie(Guid actorId, Movie movie)
        {
            try
            {
                var actor = await _actorService.CheckIfActorExists(actorId);
                if (actor == null)
                    throw new Exception("Actor with id " + actorId + " does not exist");
                if(movie.Actors.Any(x=>x.Id == actor.Id))
                    throw new Exception("Actor with id " + actorId + " is already on the list.");

                movie.Actors.Add(actor);
                await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }

        public async Task<string> AddNewActorToMovie(AddActorDto actorDto, Movie movie)
        {
            try
            {
                var addActorResponse = await _actorService.AddNewActor(actorDto) ?? throw new Exception("Actor could not be created.");

                movie.Actors.Add(addActorResponse);
                await _db.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }

        public async Task<ServiceResponse<bool>> DeleteActorFromMovie(Guid actorId, Guid movieId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                //var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == actorId) ?? throw new Exception("Actor not found.");
                var movie = await _db.Movies.Include(x => x.Actors).FirstOrDefaultAsync(x => x.Id == movieId) ?? throw new Exception("Movie not found.");
                var toDelete = movie.Actors.FirstOrDefault(x => x.Id == actorId);
                if(toDelete != null)
                 movie.Actors.Remove(toDelete);
                await _db.SaveChangesAsync();
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<GetMovieDto>> AddActortToMovie(Guid movieId, AddActorsToMovie request)
        {
            var response = new ServiceResponse<GetMovieDto>();
            try
            {
                var movieActor = await _db.Movies.Include(x => x.Actors).FirstOrDefaultAsync(x => x.Id == movieId);
                //handling actors 
                //list of actor Ids 
                if (request.ActorIds.Count > 0)
                {
                    foreach (var actorId in request.ActorIds)
                    {
                        var ex = await AddActorToMovie(actorId, movieActor);
                        if (ex == null)
                            response.Message += ex;

                    }
                }
                //list of new actors that needs to be added to database

                foreach (var actorDto in request.NewActors)
                {
                    var ex = await AddNewActorToMovie(actorDto, movieActor);
                    if (ex == null)
                        response.Message += ex;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }


            return response;
        }
    }
}
