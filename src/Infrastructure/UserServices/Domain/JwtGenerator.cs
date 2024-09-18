using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.UserServices.Domain;

internal sealed class JwtGenerator(IOptions<JwtAuthOptions> options) : IJwtGenerator
{
    private readonly JwtAuthOptions _options = options.Value;

    public JwtValue GetJwt(UserId id, Username username, Roles roles)
    {
        var expireTime = DateTime.UtcNow.Add(TimeSpan.FromSeconds(_options.Expires));

        List<Claim> claims =
        [
            new("id", id.Value.ToString()),
            new("username", username.Value),
            .. roles.Select(r => new Claim("role", r.ToString())),
        ];

        byte[] secBytes = Encoding.Default.GetBytes(_options.SecKey);
        SigningCredentials credentials =
            new(new SymmetricSecurityKey(secBytes), _options.Algorithm);

        JwtSecurityToken tokenDescriptor =
            new(claims: claims, expires: expireTime, signingCredentials: credentials);

        string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        return new(jwt);
    }
}
