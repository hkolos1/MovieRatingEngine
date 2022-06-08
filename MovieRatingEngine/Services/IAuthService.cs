using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.User;
using MovieRatingEngine.Models;
using System;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<Guid>> Register(UserAddDto req);
        Task<ServiceResponse<string>> Login(UserLoginDto req);
        Task<bool> UserExists(string username);
    }
}
