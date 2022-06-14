using Microsoft.AspNetCore.Http;
using MovieRatingEngine.Dtos.Movie;

namespace MovieRatingEngine.Helpers
{
    public interface IImageHelper
    {
        void SetImageSource(GetMovieDto result);
        string SaveImage(IFormFile imageFile);
        void DeleteImage(string imageName);
    }
}
