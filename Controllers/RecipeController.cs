using ArangoDBNetStandard.AnalyzerApi.Models;
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
    public class RecipeController : ControllerBase
    {
        private readonly RecipeRepo _recipeRepo;
        public RecipeController(RecipeRepo recipeRepo)
        {
            _recipeRepo = recipeRepo;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<RecipeModel>> GetAll([FromQuery] int time, [FromQuery] ComparisonType comparison)
        {
            return await _recipeRepo.GetAll();
        }

        [HttpGet]
        [Route("filter/name/{segment}")]
        public async Task<IEnumerable<RecipeModel>> GetAllByNameCut(string segment)
        {
            return await _recipeRepo.GetAllByNameCut(segment);
        }

        [HttpGet]
        [Route("filter/time")]
        public async Task<IEnumerable<RecipeModel>> GetAllByPrepTime([FromQuery] int time, [FromQuery] ComparisonType comparison)
        {
            if (time == 0)
            {
                return await _recipeRepo.GetAll();
            }

            return await _recipeRepo.GetByPrepTime(time, comparison);
        }

        [HttpGet]
        [Route("extended/all")]
        public async Task<IEnumerable<RecipeModel>> GetAllExtended()
        {
            return await _recipeRepo.GetAllExtended();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IEnumerable<RecipeModel>> AddRecipe([FromForm] AddRecipeRequest requestData)
        {
            try
            {
                var displayImageName = UploadUtility.GenerateFileImageName(requestData.DisplayImage.ContentType);

                var savedRecipe =  await _recipeRepo.AddRecipe(requestData, displayImageName);
                
                await UploadUtility.SaveImageFile(savedRecipe.First()._key,displayImageName,requestData.DisplayImage);
                return savedRecipe;
            }
            catch (Exception e)
            {
                return (IEnumerable<RecipeModel>)StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("filter/by-ingredients")]
        public async Task<IEnumerable<RecipeModel>> GetByIngredients([FromBody] string[] ingredientsKeys)
        {
            return await _recipeRepo.GetByIngredients(ingredientsKeys);
        }
    }
}
