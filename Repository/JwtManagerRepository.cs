using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TastyBytesReact.Models.Requests;
using TastyBytesReact.Models.Responses;

namespace TastyBytesReact.Repository
{
    public class JwtManagerRepository : IJwtManagerRepository
    {
        private readonly IConfiguration configuration;
        Dictionary<string, string> UserRecords = new Dictionary<string, string>{
            { "user1", "password1"},
            { "user2", "password2"},
            { "user3", "password3"},
            { "user4", "password4"},
            { "user5", "password5"},
            { "user6", "password6"},
        };

        public JwtManagerRepository(IConfiguration configuration) {
         this.configuration = configuration;
        }
        public TokenResponse Authenticate(LoginRequest user)
        {
            if (!UserRecords.Any(x => x.Key == user.Username && x.Value == user.Password))
            {
                return null;
            }

            //else we generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(configuration.GetSection("JWT")["Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(ClaimTypes.Name, user.Username)
                    }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenResponse { Token = tokenHandler.WriteToken(token) };
        }
    }
}
