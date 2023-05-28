using Microsoft.Extensions.DependencyInjection;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Services;

namespace Skuld.WebApi.Infrastructure.Configuration
{
	public static class DependencyInjectionConfiguration
	{
		public static IServiceCollection AddCustomDependencyInjection (this IServiceCollection services)
		{
			services.AddScoped<UnitOfWork> ()
				.AddScoped<UserService> ();

			return services;
		}
	}
}
