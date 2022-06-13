using System;

namespace MovieRatingEngine.Entity
{
    public class Rating
    {
        public Movie Movie { get; set; }
        public Guid MovieId { get; set; }
        public User User { get; set; }
        public Guid UserId{ get; set; }
        public int YourRating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
