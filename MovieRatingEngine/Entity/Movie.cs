using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRatingEngine.Entity
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double AverageRating { get; set; }
        public string Type { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<Actor> Actors { get; set; }
       

        public Guid UserId { get; set; }
        public User User { get; set; }

        public byte[] ImageByteArray { get; set; }
        public string ImageName { get; set; }

        public List<Rating> Ratings { get; set; }

    }
}
