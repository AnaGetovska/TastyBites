using TastyBytesReact.Models.Responses;

namespace TastyBytesReact.Resources
{
    public sealed record UserResponse(string Username, TokenResponse Tokens, bool IsAdmin);
}
