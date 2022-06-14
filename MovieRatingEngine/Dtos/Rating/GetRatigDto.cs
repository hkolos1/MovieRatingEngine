using MovieRatingEngine.Entity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRatingEngine.Dtos.Rating
{
    public class GetRatigDto
    {
        public Guid MovieId{ get; set; }
        
        public Guid UserId { get; set; }
       
        public string Username { get; set; }
        public string Title { get; set; }
        public int YourRating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
