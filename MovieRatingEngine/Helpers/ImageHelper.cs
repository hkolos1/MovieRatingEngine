using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MovieRatingEngine.Dtos;
using MovieRatingEngine.Models;
using System;
using System.IO;
using System.Linq;

namespace MovieRatingEngine.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _hostEnviroment;
        private readonly string _path = "https://localhost:44376/Images/";

        public ImageHelper(IWebHostEnvironment hostEnviroment)
        {
            _hostEnviroment = hostEnviroment;
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
                result.ImageSource = _path + result.ImageName;
            else
                result.ImageSource = _path + "noimageavailable.png";
        }
    }
}
