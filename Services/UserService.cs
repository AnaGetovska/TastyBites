using TastyBytesReact.Resources;
using TastyBytesReact.Models;
using ArangoDBNetStandard;
using TastyBytesReact.Data;
using TastyBytesReact.Repository.Arango;
using TastyBytesReact.Repository;

namespace TastyBytesReact.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepo _repo;
        private readonly string _pepper;
        private readonly int _iteration = 3;
        private readonly IJwtManagerRepo _jwtManagerRepository;

        public UserService(UserRepo repo, IConfiguration config, IJwtManagerRepo jwtManagerRepository)
        {
            _repo = repo;
            var passwordConfig = config.GetRequiredSection("UserPassword");
            _pepper = passwordConfig["Pepper"];
            _jwtManagerRepository = jwtManagerRepository;
        }

        public async Task Register(RegisterRequest resource)
        {
            var user = new UserModel
            {
                Username = resource.Username,
                Email = resource.Email,
                PasswordSalt = PasswordHasher.GenerateSalt(),
                IsAdmin = false
            };
            user.PasswordHash = PasswordHasher.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
            try
            {
                await _repo.CreateUser(user);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<UserResponse> Login(LoginRequest resource)
        {
            var user = await _repo.GetUserByUsername(resource.Username);

            if (user == null)
                throw new Exception("Username or password did not match.");

            var passwordHash = PasswordHasher.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
            if (user.PasswordHash != passwordHash)
                throw new Exception("Username or password did not match.");

            var token = _jwtManagerRepository.GenerateToken(resource);
            return new UserResponse(user.Username, token, user.IsAdmin);
        }
    }
}
