using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Requests;
using TastyBytesReact.Repository;

namespace TastyBytesReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IJwtManagerRepo _jwtManagerRepository;
        public UsersController(IJwtManagerRepo jwtManagerRepository)
        {
            _jwtManagerRepository = jwtManagerRepository;
        }

        [HttpGet]
        [Route("userlist")]
        public List<string> Get()
        {
            var users = new List<string>
            {
                "Ani Banani",
                "Mike Alabala",
                "david John"
            };

            return users;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate(LoginRequest request)
        {
            var token = _jwtManagerRepository.Authenticate(request);

            if(token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
