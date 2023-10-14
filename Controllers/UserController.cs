using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
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
        public UserController(IUserService service)
        {
            _service = service;
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
    }
}
