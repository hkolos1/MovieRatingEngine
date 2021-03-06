using System;
using System.Collections.Generic;

namespace MovieRatingEngine.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }

        public  List<Rating> Ratings { get; set; }


    }
}
