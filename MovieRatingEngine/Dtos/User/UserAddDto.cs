using MovieRatingEngine.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRatingEngine.Dtos.User
{
    public class UserAddDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public Role Role { get; set; }
    }
}
