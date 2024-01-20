using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TastyBytesReact.Models.Nodes;
using TastyBytesReact.Models.Responses;
using TastyBytesReact.Resources;

namespace TastyBytesReact.Repository
{
    public class JwtManagerRepo : IJwtManagerRepo
    {
        private readonly IConfiguration configuration;

        public JwtManagerRepo(IConfiguration configuration) {
         this.configuration = configuration;
        }
        public TokenResponse GenerateToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(configuration.GetSection("JWT")["Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("UserKey", user._key)
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenResponse { Token = tokenHandler.WriteToken(token) };
        }
    }
}
