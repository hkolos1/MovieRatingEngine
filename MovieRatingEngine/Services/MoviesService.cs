using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.Data;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public class MoviesService : IMoviesService
    {
       
        private readonly IMapper _mapper;
        private readonly MovieContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoviesService(IMapper mapper, MovieContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<ServiceResponse<List<GetMovieDto>>> GetAllMovies()
        {
            var serviceResponse = new ServiceResponse<List<GetMovieDto>>();
            serviceResponse.Data = _context.Movies.Select(c => _mapper.Map<GetMovieDto>(c)).ToList();
            return await Task.FromResult(serviceResponse);
        }

        public async Task<ServiceResponse<GetMovieDto>> GetMovieById(int id)
        {
            var serviceResponse = new ServiceResponse<GetMovieDto>();
            var dbCharacter = await _context.Movies.FirstOrDefaultAsync(c => c.Id == c.Id);
            serviceResponse.Data = _mapper.Map<GetMovieDto>(dbCharacter);
            return serviceResponse;
        }

        private Guid GetUserId() => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetMovieDto>>> AddMovie(AddMovieDto newMovie)
        {
            var serviceResponse = new ServiceResponse<List<GetMovieDto>>();
            Movie movie = _mapper.Map<Movie>(newMovie);
            movie.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            movie.UserId = GetUserId();
            _context.Movies.Add(movie);

            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Movies.Select(c => _mapper.Map<GetMovieDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetMovieDto>> UpdateMovie(UpdateMovieDto updatedMovie)
        {
            var serviceResponse = new ServiceResponse<GetMovieDto>();
            try
            {

                Movie movie = await _context.Movies.FirstOrDefaultAsync(c => c.Id == updatedMovie.Id);

                movie.Title = updatedMovie.Title;
                movie.Description = updatedMovie.Description;
                movie.Type = updatedMovie.Type;
                movie.ReleaseDate = updatedMovie.ReleaseDate;
                movie.PhotoUrl = updatedMovie.PhotoUrl;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetMovieDto>(movie);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetMovieDto>>> DeleteMovie(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetMovieDto>>();
            try
            {
                Movie movie = await _context.Movies.FirstAsync(c => c.Id == c.Id);
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _context.Movies.Select(c => _mapper.Map<GetMovieDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

    }

}
