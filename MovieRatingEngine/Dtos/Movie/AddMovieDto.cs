using Microsoft.AspNetCore.Http;
using MovieRatingEngine.Dtos.Actor;
using MovieRatingEngine.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRatingEngine.Dtos.Movie
{
    public class AddMovieDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Category Type { get; set; }
        public DateTime ReleaseDate { get; set; }
        //add existing actors to movie
        public List<Guid> ActorIds { get; set; }
        //add new actors to movie

        public List<AddActorDto> NewActors { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
