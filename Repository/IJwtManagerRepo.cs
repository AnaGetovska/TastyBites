using TastyBytesReact.Models.Nodes;
using TastyBytesReact.Models.Responses;
using TastyBytesReact.Resources;

namespace TastyBytesReact.Repository
{
    public interface IJwtManagerRepo
    {
        TokenResponse GenerateToken(UserModel user);
    }
}
