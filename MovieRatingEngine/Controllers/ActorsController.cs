using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Actor;
using MovieRatingEngine.Models;
using MovieRatingEngine.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRatingEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService ?? throw new ArgumentNullException(nameof(actorService));
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult<ServiceResponse<List<GetActorDto>>>> AddActor(AddActorDto request)
        {
            return StatusCode(StatusCodes.Status201Created, await _actorService.AddActor(request));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult<ServiceResponse<List<GetActorDto>>>> UpdateActor(Guid id, AddActorDto request)
        {
            var response = await _actorService.UpdateActor(id, request);
            if (response.Data != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        [HttpPatch]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult> PartiallyUpdateActor(Guid id, [FromBody] JsonPatchDocument<AddActorDto> patchDocument)
        {
            
            return Ok( await _actorService.PartiallyUpdateActor(id,patchDocument));
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = nameof(Role.Admin) + ", " + nameof(Role.User))]
        public async Task<ActionResult<ServiceResponse<List<GetActorDto>>>> GetAll()
        {
            return Ok(await _actorService.GetAll());
        }

        
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(Role.Admin) + ", " + nameof(Role.User))]
        public async Task<ActionResult<ServiceResponse<GetActorDto>>> GetById(Guid id)
        {
            return Ok(await _actorService.GetById(id));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult<ServiceResponse<GetActorDto>>> DeleteActor(Guid id)
        {
            var response = await _actorService.DeleteActor(id);
            if (response.Data != null)
                return Ok(response);
            else
                return NotFound(response);
        }

    }
}
