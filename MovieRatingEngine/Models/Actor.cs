using System;
using System.Collections.Generic;

namespace MovieRatingEngine.Models
{
    public class Actor
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Movie> Movies{ get; set; }
    }
}
