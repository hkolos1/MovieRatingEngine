using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.Models;
using System.Diagnostics.CodeAnalysis;

namespace MovieRatingEngine.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }
        DbSet<User> Users { get; set; }
        DbSet<Movie> Movies { get; set; }
        DbSet<Actor> Actors { get; set; }
    }
}
