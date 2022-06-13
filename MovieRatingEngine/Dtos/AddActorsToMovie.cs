using MovieRatingEngine.Dtos.Actor;
using System;
using System.Collections.Generic;

namespace MovieRatingEngine.Dtos
{
    public class AddActorsToMovie
    {
        public List<Guid> ActorIds{ get; set; }
        public List<AddActorDto> NewActors { get; set; }
    }
}
