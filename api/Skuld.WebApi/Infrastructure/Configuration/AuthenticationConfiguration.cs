using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Skuld.WebApi.Infrastructure.Configuration.Options;
using System;
using System.Text;

namespace Skuld.WebApi.Infrastructure.Configuration
{
	public static class AuthenticationConfiguration
	{
		public static IServiceCollection AddCustomAuthentication (this IServiceCollection services, IConfiguration configuration)
		{
			var jwtOptions = configuration
				.GetSection (JwtOptions.SectionName)
				.Get<JwtOptions> ();

			services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer (options =>
					{
						options.SaveToken = true;
						options.TokenValidationParameters = new TokenValidationParameters ()
						{
							ValidateIssuer = true,
							ValidateAudience = true,
							ValidateLifetime = true,
							ValidateIssuerSigningKey = true,
							ValidIssuer = jwtOptions.Issuer,
							ValidAudience = jwtOptions.Audience,
							IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (jwtOptions.SecretKey ?? throw new Exception ("No Secret key found in settings"))), // TODO FCU : better handling here ?
						};
					});

			return services;
		}
	}
}
