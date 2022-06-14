using Microsoft.AspNetCore.Http;
using MovieRatingEngine.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRatingEngine.Dtos.Movie
{
    public class GetMovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double AverageRating { get; set; }
        public string Type { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<Actor.GetActorDto> Actors { get; set; }
        //public string PhotoUrl { get; set; }

        // public byte[] ImageByteArray { get; set; }
        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string ImageSource { get; set; }

    }
}
