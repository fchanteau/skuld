using Microsoft.IdentityModel.Tokens;
using Skuld.Data.Entities;
using Skuld.Shared.Infrastructure.Configuration.Options;
using Skuld.Shared.Infrastructure.Constants;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Skuld.Shared.Helpers
{
    public static class TokenHelper
    {
        public static string CreateToken(User user, JwtOptions options)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(options.Issuer,
                                             options.Audience,
                                             expires: DateTime.Now.AddMinutes(30),
                                             signingCredentials: credentials);

            token.Payload.AddClaim(new Claim(CustomClaimTypes.UserId, user.UserId.ToString()));
            token.Payload.AddClaim(new Claim(CustomClaimTypes.UserName, $"{user.FirstName} {user.LastName}"));
            token.Payload.AddClaim(new Claim(CustomClaimTypes.UserEmail, $"{user.Email}"));
            token.Payload.AddClaim(new Claim(CustomClaimTypes.UserRole, $"{user.Role}"));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static RefreshToken BuildRefreshToken(User user, DateTime expiredDate)
        {
            return new RefreshToken()
            {
                Value = Guid.NewGuid().ToString(),
                UserId = user.UserId,
                ExpiredAt = expiredDate,
            };
        }
    }
}
