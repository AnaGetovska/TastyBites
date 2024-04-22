using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;

namespace TastyBytesReact.Utilities
{
    public static class UploadUtility
    {
        public static string GenerateFileImageName(string contentType)
        {
            var mimeTypes = new Dictionary<string, string>()
            {
                {"image/png", "png" },
                {"image/jpeg", "jpeg" },
                {"image/webp", "webp" },
            };

            if (!mimeTypes.ContainsKey(contentType))
            {
                throw new ArgumentException("Provided content type is not supprted.");
            }
            var filename = Guid.NewGuid().ToString() + '.' + mimeTypes.First(t => (t.Key == contentType)).Value;
            return filename;
        }

        private static string CreateFileImageDirectory(string recipeKey)
        {
            //var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var imgDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", @"images", recipeKey);
            if (!Directory.Exists(imgDirectory))
            {
                var dirInfo = Directory.CreateDirectory(imgDirectory);
            }
            return imgDirectory;
        }

        public static async Task SaveImageFile(string recipeKey, string imageFileName, IFormFile file)
        {
            try
            {
                var imagePath = Path.Combine(CreateFileImageDirectory(recipeKey), imageFileName);
                using (Stream stream = new FileStream(imagePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static string ConvertImageToBase64(IFormFile imageData)
        {
            using var dataStream = imageData.OpenReadStream();
            using var reader = new BinaryReader(dataStream);
            byte[] byteData = reader.ReadBytes((int)dataStream.Length);
            return Convert.ToBase64String(byteData);
        }
    }
}
