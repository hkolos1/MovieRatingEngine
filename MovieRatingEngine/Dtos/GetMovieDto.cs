using MovieRatingEngine.Models;
using System;
using System.Collections.Generic;

namespace MovieRatingEngine.Dtos
{
    public class GetMovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double AverageRating { get; set; }
        public string Type { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<Actor> Actors { get; set; }
        public string PhotoUrl { get; set; }

    }
}
