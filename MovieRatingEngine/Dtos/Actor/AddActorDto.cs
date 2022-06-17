using System.ComponentModel.DataAnnotations;

namespace MovieRatingEngine.Dtos.Actor
{
    public class AddActorDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
