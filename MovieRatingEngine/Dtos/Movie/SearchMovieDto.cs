using MovieRatingEngine.Entity;

namespace MovieRatingEngine.Dtos.Movie
{
    public class SearchMovieDto
    {
        public string searchBar { get; set; } = null;
        public Category type { get; set; }
    }
}
