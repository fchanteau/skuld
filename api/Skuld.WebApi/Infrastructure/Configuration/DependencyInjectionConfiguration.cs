using Microsoft.Extensions.DependencyInjection;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Features.Auth;

namespace Skuld.WebApi.Infrastructure.Configuration
{
	public static class DependencyInjectionConfiguration
	{
		public static IServiceCollection AddCustomDependencyInjection (this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork> ();
			services.AddScoped<IAuthService, AuthService> ();

			return services;
		}
	}
}
