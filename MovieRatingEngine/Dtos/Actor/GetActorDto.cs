using MovieRatingEngine.Dtos.Movie;
using System;
using System.Collections.Generic;

namespace MovieRatingEngine.Dtos.Actor
{
    public class GetActorDto
    {
        public Guid Id{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<GetMovieDto> Movies{ get; set; }
    }
}
