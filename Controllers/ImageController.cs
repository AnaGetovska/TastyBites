using ArangoDBNetStandard.AnalyzerApi.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Nodes;
using TastyBytesReact.Models.Requests;
using TastyBytesReact.Repository.Arango;
using TastyBytesReact.Utilities;

namespace TastyBytesReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageRepo _imageRepo;
        public ImageController(ImageRepo imageRepo)
        {
            _imageRepo = imageRepo;
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<FileContentResult> GetImageByKey(string key)
        {
            var images = await _imageRepo.GetImageByKey(key);

            
            byte[] bytes = Convert.FromBase64String(images.FirstOrDefault().Body);
            return File(bytes, "image/png");
        }
    }
}
