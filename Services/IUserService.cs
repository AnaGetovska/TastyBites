using TastyBytesReact.Resources;

namespace TastyBytesReact.Services
{
    public interface IUserService
    {
        Task Register(RegisterRequest resource);
        Task<UserResponse> Login(LoginRequest resource);
    }
}
