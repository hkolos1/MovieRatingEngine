using Microsoft.AspNetCore.Http;
using MovieRatingEngine.Dtos.Actor;
using MovieRatingEngine.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRatingEngine.Dtos.Movie
{
    public class AddMovieDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        
        public Category Type { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        //add existing actors to movie
        public List<Guid> ActorIds { get; set; } = new List<Guid>();
        //add new actors to movie
        public List<AddActorDto> NewActors { get; set; } = new List<AddActorDto>();

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
