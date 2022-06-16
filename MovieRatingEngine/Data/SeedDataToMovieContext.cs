using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.Entity;
using MovieRatingEngine.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRatingEngine.Data
{
 
    public static class SeedDataToMovieContext
    {

        public static T GetRandomElement<T>(this List<T> list)
        {
            int x = new Random().Next(0, list.Count);
            return list[x];
        }

        public static string GetRandomString(int lenght = 3)
        {
            return Guid.NewGuid().ToString().Substring(0, lenght);
        }
        public static DateTime RandomDay()
        {
            DateTime start = new DateTime(1990, 1, 1);
            Random gen = new Random();
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        public static async Task<string> Generate(MovieContext db, IAuthService _authService)
        {
            var Admin = await _authService.Register(new Dtos.User.UserAddDto
            {
                FirstName = "User",
                LastName = "Admin",
                Role = Role.Admin,
                Username = "admin"+new Random().Next(200),
                Password = "admin"
            });
            var Movies = new List<Movie>();
            var Actors = new List<Actor>();

            for (int i = 0; i < 5; i++)
            {
                Movies.Add(new Movie
                {
                    Title = "Title "+GetRandomString(5),
                    Description = GetRandomString(10),
                    Type = Category.Movie.ToString(),
                    ReleaseDate = RandomDay(),
                    ImageName = "noimageavailable.png",
                    UserId = Admin.Data
                });
            }

            for (int i = 0; i < 5; i++)
            {
                Movies.Add(new Movie
                {
                    Title = "Title " + GetRandomString(5),
                    Description = GetRandomString(10),
                    Type = Category.TvShow.ToString(),
                    ReleaseDate = RandomDay(),
                    ImageName = "noimageavailable.png",
                    UserId = Admin.Data
                });
            }

            for (int i = 0; i < 10; i++)
            {
                Actors.Add(new Actor
                {
                    FirstName = "Name" + GetRandomString(4),
                    LastName = "Surname" + GetRandomString(4)
                });
            }
            await db.Movies.AddRangeAsync(Movies);
            await db.Actors.AddRangeAsync(Actors);
            await db.SaveChangesAsync();

            var dbMovies = await db.Movies.Include(x => x.Actors).ToListAsync();
            foreach (var movie in db.Movies)
            {
                for (int i = 0; i < 2; i++)
                {
                    movie.Actors.Add(GetRandomElement<Actor>(Actors));
                }
            }
            await db.SaveChangesAsync();
            var dbAdmin = await db.Users.FirstOrDefaultAsync(x => x.Id == Admin.Data);

            return "Test admin credentials: username->"+dbAdmin.Username+"  password->admin" ;
        }
    }
}
