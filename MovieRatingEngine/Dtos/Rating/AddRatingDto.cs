using System;
using System.ComponentModel.DataAnnotations;

namespace MovieRatingEngine.Dtos.Rating
{
    public class AddRatingDto
    {
        [Required]
        public Guid MovieId { get; set; }
        [Required]
        public int YourRating { get; set; }
    }
}
