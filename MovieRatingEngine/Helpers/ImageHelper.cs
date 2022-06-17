using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MovieRatingEngine.Dtos.Movie;
using System;
using System.IO;
using System.Linq;

namespace MovieRatingEngine.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _hostEnviroment;
        private readonly IConfiguration _configuration;

        public ImageHelper(IWebHostEnvironment hostEnviroment,IConfiguration configuration)
        {
            _hostEnviroment = hostEnviroment;
            _configuration = configuration;
        }

        public void DeleteImage(string imageName)
        {
            if (imageName != null)
            {
                var imagePath = Path.Combine(_hostEnviroment.ContentRootPath, "Images", imageName);
                if (System.IO.File.Exists(imagePath) && imageName != "noimageavailable.png")
                    System.IO.File.Delete(imagePath);
            }
        }
        public string SaveImage(IFormFile imageFile)
        {
            var imageName = "";
            if (imageFile == null)
            {
                imageName = "noimageavailable.png";
            }
            else
            {
                imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(_hostEnviroment.ContentRootPath, "Images", imageName);
                using (var filestream = new FileStream(imagePath, FileMode.Create))
                {
                    imageFile.CopyTo(filestream);
                }
            }

            return imageName;

        }

        public void SetImageSource(GetMovieDto result)
        {
            if (result.ImageName != null)
                result.ImageSource = _configuration.GetSection("AppSettings:ImagePath").Value + result.ImageName;
            else
                result.ImageSource = _configuration.GetSection("AppSettings:ImagePath").Value + result.ImageName;
        }
    }
}
