using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Skuld.Data.Entities;
using Skuld.WebApi.Common.Configuration.Options;
using Skuld.WebApi.Common.Constants;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Skuld.WebApi.Helpers;

public interface ITokenProvider
{
	string CreateToken (User user);
	RefreshToken BuildRefreshToken (User user, DateTime expiredDate);
}

public class TokenProvider : ITokenProvider
{
	private readonly JwtOptions _jwtOptions;
	private readonly IDateTimeProvider _dateTimeProvider;

	public TokenProvider (IOptions<JwtOptions> jwtOptions, IDateTimeProvider dateTimeProvider)
	{
		_jwtOptions = jwtOptions.Value;
		_dateTimeProvider = dateTimeProvider;
	}

	public string CreateToken (User user)
	{
		// TODO FCU : Maybe better handling here ?
		var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_jwtOptions.SecretKey ?? throw new Exception ("No Secret key found in settings")));
		var credentials = new SigningCredentials (key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken (_jwtOptions.Issuer,
										 _jwtOptions.Audience,
										 expires: _dateTimeProvider.UtcNow.AddMinutes (60),
										 signingCredentials: credentials);

		token.Payload.AddClaim (new Claim (CustomClaimTypes.UserId, user.UserId.ToString ()));
		token.Payload.AddClaim (new Claim (CustomClaimTypes.UserName, $"{user.FirstName} {user.LastName}"));
		token.Payload.AddClaim (new Claim (CustomClaimTypes.UserEmail, $"{user.Email}"));
		token.Payload.AddClaim (new Claim (CustomClaimTypes.UserRole, $"{user.Role}"));

		return new JwtSecurityTokenHandler ().WriteToken (token);
	}

	public RefreshToken BuildRefreshToken (User user, DateTime expiredDate)
	{
		return new RefreshToken ()
		{
			Value = Guid.NewGuid ().ToString (),
			UserId = user.UserId,
			ExpiredAt = expiredDate,
		};
	}
}
