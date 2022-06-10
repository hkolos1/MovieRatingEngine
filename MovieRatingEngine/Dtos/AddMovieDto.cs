using Microsoft.AspNetCore.Http;
using MovieRatingEngine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRatingEngine.Dtos
{
    public class AddMovieDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime ReleaseDate { get; set; }

        //public List<Actor> Actors { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
