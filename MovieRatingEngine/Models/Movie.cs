using System;
using System.Collections.Generic;

namespace MovieRatingEngine.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal AverageRating { get; set; }
        public string Type { get; set; }
        public DateTime Released { get; set; }
        public List<string> Cast { get; set; }

    }
}
