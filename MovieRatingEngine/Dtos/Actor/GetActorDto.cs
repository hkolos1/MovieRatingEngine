using System;

namespace MovieRatingEngine.Dtos.Actor
{
    public class GetActorDto
    {
        public Guid Id{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
