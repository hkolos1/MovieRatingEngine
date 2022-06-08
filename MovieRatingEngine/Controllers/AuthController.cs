using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.User;
using MovieRatingEngine.Services;
using System;
using System.Threading.Tasks;

namespace MovieRatingEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<Guid>>> Register(UserAddDto request)
        {
            var response = await _authService.Register(request);

            if (!response.Success)

            {
                return BadRequest(response);
            }

            return Ok(response);
        }
     

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _authService.Login(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
