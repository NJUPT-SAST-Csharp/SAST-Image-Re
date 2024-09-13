using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.UserServices.Domain
{
    internal sealed class JwtGenerator(IOptions<JwtAuthOptions> options) : IJwtGenerator
    {
        private readonly JwtAuthOptions _options = options.Value;

        public JwtValue GetJwt(UserId id, Username username, Roles roles)
        {
            DateTime expireTime = DateTime.UtcNow.Add(TimeSpan.FromSeconds(_options.Expires));

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, id.Value.ToString()),
                new(ClaimTypes.Name, username.Value),
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.ToString())));

            byte[] secBytes = Encoding.Default.GetBytes(_options.SecKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(secBytes),
                _options.Algorithm
            );

            var tokenDescriptor = new JwtSecurityToken(
                claims: claims,
                expires: expireTime,
                signingCredentials: credentials
            );

            string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return new(jwt);
        }
    }
}
