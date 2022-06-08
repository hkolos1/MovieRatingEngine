using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.Models;
using System.Diagnostics.CodeAnalysis;

namespace MovieRatingEngine.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions options) : base(options)
        {
        }

      
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
    }
}
