using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TastyBytesReact.Models;
using TastyBytesReact.Repository.Arango;

namespace TastyBytesReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepo _categoryRepo;

        public CategoryController(CategoryRepo categoryRepo) {
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<CategoryModel>> GetAllCategories() {
            return await _categoryRepo.GetAll();
        }

        [HttpGet]
        [Route("{type}/all")]
        public async Task<IEnumerable<CategoryModel>> GetAllCategories(CategoryType type)
        {
            return await _categoryRepo.GetAll(type);
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<IEnumerable<CategoryModel>> GetCategoryByKey(string key)
        {
            return await _categoryRepo.GetByKey(key);
        }
    }
}
