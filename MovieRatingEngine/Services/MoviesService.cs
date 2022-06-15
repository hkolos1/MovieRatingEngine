using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.Data;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Dtos.Movie;
using MovieRatingEngine.Entity;
using MovieRatingEngine.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieRatingEngine.Services
{
    public class MoviesService : IMoviesService
    {

        private readonly IMapper _mapper;
        private readonly MovieContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageHelper _imageHelper;
        private readonly IActorMovieService _actorMovieService;
        private readonly IActorService _actorService;
        private readonly List<string> _genericSearchWords = new List<string> { "star", "at least", "year", "after", "older than" };

        public MoviesService(IMapper mapper, MovieContext context, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper, IActorMovieService actorMovieService, IActorService actorService)

        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _imageHelper = imageHelper;
            _actorMovieService = actorMovieService;
            _actorService = actorService;
        }

        public async Task<ServiceResponse<List<GetMovieDto>>> GetAllMovies()
        {
            var serviceResponse = new ServiceResponse<List<GetMovieDto>>();
            try
            {
                var mapped = await _context.Movies.Include(x => x.Actors).Select(c => _mapper.Map<GetMovieDto>(c)).ToListAsync();
                foreach (var movie in mapped)
                {
                    _imageHelper.SetImageSource(movie);
                }
                serviceResponse.Data = mapped;

            }
            catch (Exception ex)
            {

                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return await Task.FromResult(serviceResponse);
        }

        public async Task<ServiceResponse<GetMovieDto>> GetMovieById(Guid id)
        {
            var serviceResponse = new ServiceResponse<GetMovieDto>();
            try
            {
                var dbCharacter = await _context.Movies.Include(x => x.Actors).FirstOrDefaultAsync(c => c.Id == id);
                if (dbCharacter == null)
                    throw new Exception("Movie not found.");
                var mapped = _mapper.Map<GetMovieDto>(dbCharacter);
                _imageHelper.SetImageSource(mapped);
                serviceResponse.Data = mapped;

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        private Guid GetUserId() => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetMovieDto>>> AddMovie(AddMovieDto newMovie)//, List<AddActorDto> addNewActors)
        {
            var serviceResponse = new ServiceResponse<List<GetMovieDto>>();
            try
            {
                Movie movie = _mapper.Map<Movie>(newMovie);
                movie.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
                movie.UserId = GetUserId();

                if (newMovie.Type == Category.Movie)
                    movie.Type = Category.Movie.ToString();
                else
                    movie.Type = Category.TvShow.ToString();

                movie.ImageName = _imageHelper.SaveImage(newMovie.ImageFile);

                if (newMovie.ImageFile != null)
                {
                    MemoryStream ms = new MemoryStream();
                    newMovie.ImageFile.CopyTo(ms);
                    movie.ImageByteArray = ms.ToArray();
                }
                _context.Movies.Add(movie);

                await _context.SaveChangesAsync();

                var movieActor = await _context.Movies.Include(x => x.Actors).FirstOrDefaultAsync(x => x == movie);
                //handling actors 
                //list of actor Ids 
                if (newMovie.ActorIds.Count > 0)
                {
                    foreach (var actorId in newMovie.ActorIds)
                    {
                        var ex = await _actorMovieService.AddActorToMovie(actorId, movieActor);
                        if (ex == null)
                            serviceResponse.Message += ex;

                    }
                }
                //list of new actors that needs to be added to database

                foreach (var actorDto in newMovie.NewActors)
                {
                    var ex = await _actorMovieService.AddNewActorToMovie(actorDto, movieActor);
                    if (ex == null)
                        serviceResponse.Message += ex;
                }

                var mapped = await _context.Movies.Select(c => _mapper.Map<GetMovieDto>(c)).ToListAsync();
                foreach (var m in mapped)
                {
                    _imageHelper.SetImageSource(m);
                }
                serviceResponse.Data = mapped;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
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
                movie.Type = updatedMovie.Type.ToString();
                movie.ReleaseDate = updatedMovie.ReleaseDate;

                if (updatedMovie.ImageFile != null)
                {
                    //delete from file
                    _imageHelper.DeleteImage(movie.ImageName);
                    movie.ImageName = _imageHelper.SaveImage(updatedMovie.ImageFile);


                    MemoryStream ms = new MemoryStream();
                    updatedMovie.ImageFile.CopyTo(ms);
                    movie.ImageByteArray = ms.ToArray();
                }


                await _context.SaveChangesAsync();
                var movieActor = await _context.Movies.Include(x => x.Actors).FirstOrDefaultAsync(x => x == movie);
                //handling actors 
                //list of actor Ids 
                if (updatedMovie.ActorIds.Count > 0)
                {
                    foreach (var actorId in updatedMovie.ActorIds)
                    {
                        var ex = await _actorMovieService.AddActorToMovie(actorId, movieActor);
                        if (ex == null)
                            serviceResponse.Message += ex;

                    }
                }
                //list of new actors that needs to be added to database

                foreach (var actorDto in updatedMovie.NewActors)
                {
                    var ex = await _actorMovieService.AddNewActorToMovie(actorDto, movieActor);
                    if (ex == null)
                        serviceResponse.Message += ex;
                }


                var mapped = _mapper.Map<GetMovieDto>(movie);
                _imageHelper.SetImageSource(mapped);
                serviceResponse.Data = mapped;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetMovieDto>>> DeleteMovie(Guid id)
        {
            var serviceResponse = new ServiceResponse<List<GetMovieDto>>();
            try
            {
                Movie movie = await _context.Movies.FirstAsync(c => c.Id == id);
                if (movie == null)
                    throw new Exception("Movie not found.");

                _imageHelper.DeleteImage(movie.ImageName);
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

        public async Task<List<GetMovieDto>> PagingMovie(int? pageNumber, int? pageSize)
        {
            var movies = await _context.Movies.Include(x => x.Actors).Select(c => _mapper.Map<GetMovieDto>(c)).ToListAsync();
            foreach (var movie in movies)
            {
                _imageHelper.SetImageSource(movie);
            }
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 10;

            return movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).ToList();
        }

        public async Task<List<GetMovieDto>> SearchMovie(string searchBar, Category type)
        {

            var typeString = type == Category.Movie ? Category.Movie.ToString() : Category.TvShow.ToString();

            if (searchBar == null || searchBar.Length < 2)
            {
                var movieList = await _context.Movies.Where(x => x.Type.ToLower().Equals(typeString)).OrderByDescending(x => x.AverageRating).Take(10).ToListAsync();
                return _mapper.Map<List<GetMovieDto>>(movieList);
            }
            var queryMovies = _context.Movies.AsQueryable();
            queryMovies = queryMovies.Where(x => x.Type.ToLower().Equals(typeString));


            // recognizing phrases like "5 stars", "at least 3 stars", "after 2015", "older than 5 years"
            var listMovies =  SearchPhrases(searchBar, queryMovies);

            //add to listMovies list of movies that contains inserted search string in theirs text attributes

            listMovies.AddRange(SearchTextualAttributes(searchBar, queryMovies));
            foreach (var movie in listMovies)
            {
                _imageHelper.SetImageSource(movie);
            }
            return listMovies;
        }

        private List<GetMovieDto> SearchTextualAttributes(string searchBar, IQueryable<Movie> queryMovies)
        {
            queryMovies = queryMovies.Where(x =>
               x.Title.ToLower().Contains(searchBar.ToLower()) ||
               x.Description.ToLower().Contains(searchBar.ToLower())
               );
            queryMovies = queryMovies.Include(x => x.Actors);
            return  queryMovies.Select(x => _mapper.Map<GetMovieDto>(x)).ToList(); ;
        }

        private List<GetMovieDto> SearchPhrases(string searchBar, IQueryable<Movie> queryMovies)
        {
            if (searchBar != null)
            {
                //check if number is inserted in searchBar
                var regResultNumber = Regex.Match(searchBar, @"\d+").Value;
                if (!string.IsNullOrEmpty(regResultNumber))
                {
                    //check for the word "star"
                    var regResultStar = searchBar.Contains(_genericSearchWords[0]) ? _genericSearchWords[0] : "";
                    if (!string.IsNullOrEmpty(regResultStar))
                    {
                        //if there is phrase "at least"  do query with >=, if not take movies whose average rating equals to inserted number 
                        var regResultAtleast = searchBar.Contains(_genericSearchWords[1]);
                        if (regResultAtleast)
                            queryMovies = queryMovies.Where(x => x.AverageRating >= Int32.Parse(regResultNumber));
                        else
                            queryMovies = queryMovies.Where(x => x.AverageRating == int.Parse(regResultNumber));
                    }

                    var regResultYear = searchBar.Contains(_genericSearchWords[2]) ? _genericSearchWords[2] : "";
                    var regResultAfter = searchBar.Contains(_genericSearchWords[3]) ? _genericSearchWords[3] : "";
                    var regResultOlderThan = searchBar.Contains(_genericSearchWords[4]) ? _genericSearchWords[4] : "";

                    //if "after" i inserted in searchBar 
                    if (!string.IsNullOrEmpty(regResultAfter))
                        queryMovies = queryMovies.Where(x => x.ReleaseDate.Year > Int32.Parse(regResultNumber));
                    if (!string.IsNullOrEmpty(regResultOlderThan))
                        queryMovies = queryMovies.Where(x => x.ReleaseDate.AddYears(Int32.Parse(regResultNumber)) < DateTime.Now);

                }

            }

            queryMovies = queryMovies.Include(x => x.Actors);
           // return queryMovies;
           return queryMovies.Select(x => _mapper.Map<GetMovieDto>(x)).ToList();
        }

        public async Task<string> SetRating(Movie movie, int yourRating)
        {

            if (movie == null)
                return "Movie not found.";
            try
            {
                movie.AverageRating = Math.Round(await _context.Ratings.Where(x => x.MovieId == movie.Id).AverageAsync(x => x.YourRating), 1);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return null;
        }


        //private async Task<List<string>> Words(string searchWord)
        //{
        //    var words = new List<string>();
        //    if (string.IsNullOrEmpty(searchWord))
        //        return words;
        //    searchWord = searchWord.ToLower();

        //    var regResultNumber = Regex.Match(searchWord, @"\d+").Value;
        //    var regResultStar = searchWord.Contains(_genericSearchWords[0]) ? _genericSearchWords[0] : "";
        //    if (!string.IsNullOrEmpty(regResultStar))
        //    {
        //        var regResultAtleast = searchWord.Contains(_genericSearchWords[1]) ? _genericSearchWords[1] : "";

        //    }

        //    if (!String.IsNullOrEmpty(regResultNumber))
        //    {
        //        int number = Int32.Parse(regResultNumber);
        //    }


        //    return words;
        //}
    }

}
