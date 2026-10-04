using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Application.Features.Auth;
using CafeEmployeeManagement.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace CafeEmployeeManagement.Infrastructure.Identity
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        /// <summary>
        /// Short claim name for roles. The JWT bearer handler is configured to read roles from it.
        /// </summary>
        public const string RoleClaimType = "role";

        private readonly JwtSettings settings;

        public JwtTokenGenerator(IOptions<JwtSettings> options)
        {
            settings = options.Value;
        }

        public AuthResponseDto GenerateToken(AuthUser user)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(settings.ExpiryMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            claims.AddRange(user.Roles.Select(role => new Claim(RoleClaimType, role)));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = settings.Issuer,
                Audience = settings.Audience,
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };

            return new AuthResponseDto
            {
                Token = new JsonWebTokenHandler().CreateToken(descriptor),
                ExpiresAt = expiresAt,
                Email = user.Email,
                Roles = user.Roles,
            };
        }
    }
}
