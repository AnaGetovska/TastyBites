using TastyBytesReact.Models.Requests;
using TastyBytesReact.Models.Responses;

namespace TastyBytesReact.Repository
{
    public interface IJwtManagerRepo
    {
        TokenResponse Authenticate(LoginRequest user);
    }
}
