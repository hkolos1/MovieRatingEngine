using Microsoft.AspNetCore.JsonPatch;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Actor;
using MovieRatingEngine.Dtos.Movie;
using MovieRatingEngine.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public interface IActorService
    {
        Task<ServiceResponse<List<GetActorDto>>> AddActor(AddActorDto request);
        Task<Actor> AddNewActor(AddActorDto request);
        Task<ServiceResponse<List<GetActorDto>>> GetAll();
        Task<ServiceResponse<GetActorDto>> GetById(Guid id);
        Task<ServiceResponse<GetActorDto>> UpdateActor(Guid id, AddActorDto request);
        Task<ServiceResponse<GetActorDto>> PartiallyUpdateActor(Guid id, JsonPatchDocument<AddActorDto> patchDocument);
        Task<ServiceResponse<List<GetActorDto>>> DeleteActor(Guid id);
        Task<Actor>CheckIfActorExists(Guid actorId);

    }
}
