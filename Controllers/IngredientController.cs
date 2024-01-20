using ArangoDBNetStandard.AnalyzerApi.Models;
using Microsoft.AspNetCore.Mvc;
using TastyBytesReact.Models.Nodes;
using TastyBytesReact.Repository.Arango;

namespace TastyBytesReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IngredientRepo _ingRepo;
        public IngredientController(IngredientRepo ingRepo)
        {
            _ingRepo = ingRepo;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<IngredientModel>> GetAll()
        {
            return await _ingRepo.GetAll();
        }

        [HttpGet]
        [Route("all/extended")]
        public async Task<IEnumerable<IngredientModel>> GetAllExtended()
        {
            return await _ingRepo.GetAllExtended();
        }

        [HttpGet]
        [Route("filter/name/{segment}")]
        public async Task<IEnumerable<IngredientModel>> GetAllByNameCut(string segment)
        {
            return await _ingRepo.GetAllByNameCut(segment);
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IEnumerable<IngredientModel>> GetByKeys([FromBody] string[] ingredientsKeys)
        {
            return (IEnumerable<IngredientModel>)await _ingRepo.GetAllByKeys(ingredientsKeys);
        }
    }
}
