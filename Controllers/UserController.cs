using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Edges;
using TastyBytesReact.Repository.Arango;
using TastyBytesReact.Resources;
using TastyBytesReact.Services;

namespace TastyBytesReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly UserRepo _repo;

        public UserController(IUserService service, UserRepo repo)
        {
            _service = service;
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest resource)
        {
            try
            {
                await _service.Register(resource);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<UserResponse> Authenticate(LoginRequest request)
        {
            var userData = await _service.Login(request);

            return userData;
        }

        [HttpGet]
        [Route("menu")]
        public async Task<IEnumerable<InMenuModel>> GetWeeklyMenu()
        {
            var userKey = User.Claims.FirstOrDefault(c=> c.Type == "UserKey")?.Value;
            if(userKey == null)
            {
                throw new InvalidDataException($"User key is null.");
            }

            return await _repo.GetWeeklyMenuByUserKey(userKey);
        }

        [HttpPost]
        [Route("menu/{key}")]
        public async Task SetWeeklyMenu([FromRoute] string key, DayEnum[] days)
        {
            var userKey = User.Claims.FirstOrDefault(c => c.Type == "UserKey")?.Value;
            if (userKey == null)
            {
                throw new InvalidDataException($"User key is null.");
            }

            await _repo.SetWeeklyMenuByUserKey(userKey, key, days);
        }

        [HttpGet]
        [Route("shopping-list")]
        public async Task<IEnumerable<ShoppingListItemModel>> GetShoppingList()
        {
            var userKey = User.Claims.FirstOrDefault(c => c.Type == "UserKey")?.Value;
            if (userKey == null)
            {
                throw new InvalidDataException($"User key is null.");
            }

            return await _repo.GetShoppingList(userKey);
        }

        [HttpPost]
        [Route("shopping-list")]
        public async Task AddToShoppingList([FromBody] ShoppingListItemModel item)
        {
            var userKey = User.Claims.FirstOrDefault(c => c.Type == "UserKey")?.Value;
            if (userKey == null)
            {
                throw new InvalidDataException($"User key is null.");
            }

            await _repo.AddToShoppingList(userKey, item);
        }

        [HttpDelete]
        [Route("shopping-list/{index}")]
        public async Task DeleteFromShoppingList(int index)
        {
            var userKey = User.Claims.FirstOrDefault(c => c.Type == "UserKey")?.Value;
            if (userKey == null)
            {
                throw new InvalidDataException($"User key is null.");
            }

            await _repo.RemoveFromShoppingList(userKey,(uint) index);
        }
    }
}
