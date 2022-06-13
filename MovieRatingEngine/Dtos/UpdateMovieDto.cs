using Microsoft.AspNetCore.Http;
using MovieRatingEngine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRatingEngine.Dtos
{
    public class UpdateMovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Category Type { get; set; }
        public DateTime ReleaseDate { get; set; }

        //public List<Actor> Actors { get; set; }
        //public string PhotoUrl { get; set; }

        //public byte[] ImageByteArray { get; set; }
       // public string ImageName { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
