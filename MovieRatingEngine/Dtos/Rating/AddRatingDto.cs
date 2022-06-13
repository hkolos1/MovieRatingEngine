using System;
using System.ComponentModel.DataAnnotations;

namespace MovieRatingEngine.Dtos.Rating
{
    public class AddRatingDto
    {
        [Required]
        public Guid MovieId { get; set; }
        //public Guid UserId { get; set; } //take userId from token
        [Required]
        public int YourRating { get; set; }
    }
}
