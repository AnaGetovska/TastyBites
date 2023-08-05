using TastyBytesReact.Models.Requests;
using TastyBytesReact.Models.Responses;

namespace TastyBytesReact.Repository
{
    public interface IJwtManagerRepository
    {
        TokenResponse Authenticate(LoginRequest user);
    }
}
