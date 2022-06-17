using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.Data;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Actor;
using MovieRatingEngine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public class ActorService : IActorService
    {
        private readonly IMapper _mapper;
        private readonly MovieContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActorService(IMapper mapper, MovieContext db, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        private Guid GetUserId() => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role));

        public async Task<ServiceResponse<List<GetActorDto>>> AddActor(AddActorDto request)
        {
            var response = new ServiceResponse<List<GetActorDto>>();

            try
            {
                if (_db.Actors.Any(x => x.FirstName == request.FirstName && x.LastName == request.LastName))
                {
                    response.Success = false;
                    response.Message = "Actor already exist. ";
                }
                else
                {
                    var actor = _mapper.Map<Actor>(request);
                    await _db.Actors.AddAsync(actor);
                    await _db.SaveChangesAsync();

                    response.Data = await _db.Actors.Select(x => _mapper.Map<GetActorDto>(x)).ToListAsync();
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
        //add new actor while adding movie
        public async Task<Actor> AddNewActor(AddActorDto request)
        {
            var dbActor = await _db.Actors.Where(x => x.FirstName == request.FirstName && x.LastName == request.LastName).FirstOrDefaultAsync();
            if (dbActor != null)
                return dbActor;
            var actor = _mapper.Map<Actor>(request);
            await _db.Actors.AddAsync(actor);
            await _db.SaveChangesAsync();
            return actor;
        }

        public async Task<ServiceResponse<List<GetActorDto>>> DeleteActor(Guid id)
        {
            var response = new ServiceResponse<List<GetActorDto>>();
            try
            {
                var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == id);

                if (actor == null)
                    throw new Exception("Actor not found.");

                _db.Actors.Remove(actor);
                await _db.SaveChangesAsync();

                response.Data = await _db.Actors.Select(x => _mapper.Map<GetActorDto>(actor)).ToListAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetActorDto>>> GetAll()
        {
            var response = new ServiceResponse<List<GetActorDto>>();
            try
            {
                response.Data = await _db.Actors.Select(x => _mapper.Map<GetActorDto>(x)).ToListAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetActorDto>> GetById(Guid id)
        {
            var response = new ServiceResponse<GetActorDto>();
            try
            {
                var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == id);

                if (actor == null)
                    throw new Exception("Actor not found.");

                response.Data = _mapper.Map<GetActorDto>(actor);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetActorDto>> UpdateActor(Guid id, AddActorDto request)
        {
            var response = new ServiceResponse<GetActorDto>();
            try
            {
                var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == id);
                if (actor == null)
                    throw new Exception("Actor not found.");
                if (request.FirstName != null)
                    actor.FirstName = request.FirstName;
                if (request.LastName != null)
                    actor.LastName = request.LastName;

                await _db.SaveChangesAsync();

                response.Data = _mapper.Map<GetActorDto>(actor);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetActorDto>> PartiallyUpdateActor(Guid id, JsonPatchDocument<AddActorDto> patchDocument)
        {
            var response = new ServiceResponse<GetActorDto>();
            try
            {
                var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Actor not found.");
                var actorToPatch = _mapper.Map<AddActorDto>(actor);
                patchDocument.ApplyTo(actorToPatch);

                _mapper.Map(actorToPatch, actor);
                await _db.SaveChangesAsync();

                response.Data = _mapper.Map<GetActorDto>(actor);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<Actor> CheckIfActorExists(Guid actorId)
        {
            var actor = await _db.Actors.FirstOrDefaultAsync(x => x.Id == actorId);
            if (actor == null)
                return null;
            return actor;
        }
    }
}
