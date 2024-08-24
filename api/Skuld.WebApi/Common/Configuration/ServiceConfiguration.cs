using Microsoft.Extensions.DependencyInjection;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Features.Auth;
using Skuld.WebApi.Features.Users;

namespace Skuld.WebApi.Common.Configuration;

public static class ServiceConfiguration
{
	public static IServiceCollection AddCustomServices (this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork> ();

		services.AddScoped<IAuthService, AuthService> ();
		services.AddScoped<IUserService, UserService> ();

		return services;
	}
}
