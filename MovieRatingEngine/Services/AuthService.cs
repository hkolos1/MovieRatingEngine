using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieRatingEngine.Data;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Movie;
using MovieRatingEngine.Dtos.User;
using MovieRatingEngine.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public class AuthService:IAuthService
    {
        private readonly MovieContext _db;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(MovieContext db, IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Login(UserLoginDto req)//, string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == req.Username.ToLower());
            
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(req.Password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;
        }

        public async Task<ServiceResponse<Guid>> Register(UserAddDto req)//, User user, string password)
        {
            ServiceResponse<Guid> response = new ServiceResponse<Guid>();

            if (await UserExists(req.Username))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }
            if (!Enum.IsDefined(typeof(Role), req.Role)){
                response.Success = false;
                response.Message = "Role doesn't exist.";
                return response;
            }

            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = _mapper.Map<Entity.User>(req);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            if(req.Role==Role.Admin)
                user.Role=Role.Admin.ToString();
            else
                user.Role=Role.User.ToString();

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _db.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
                return true;
            return false;
        }



        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }

                return true;
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role,user.Role)

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
