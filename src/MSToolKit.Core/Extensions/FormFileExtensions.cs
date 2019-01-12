using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MSToolKit.Core.Extensions
{
    public static class FormFileExtensions
    {
        public static bool IsImage(this IFormFile formFile)
        {
            var hasCorrectExtension = false;
            var hasCorrectContentType = formFile.ContentType.Contains("image");
            var fileExtension = formFile.FileName.Split('.').Last();
            switch (fileExtension.ToLower())
            {
                case "jpg":
                case "jpeg":
                case "png":
                case "bmp":
                case "gif":
                    hasCorrectExtension = true;
                    break;
                default:
                    hasCorrectExtension = false;
                    break;
            }
            
            return hasCorrectExtension && hasCorrectContentType;
        }
    }
}
