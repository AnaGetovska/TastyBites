using ArangoDBNetStandard.AnalyzerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Nodes;
using TastyBytesReact.Repository.Arango;

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
    }
}
