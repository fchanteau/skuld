using Microsoft.Extensions.DependencyInjection;
using Skuld.WebApi.Helpers;

namespace Skuld.WebApi.Common.Configuration
{
	public static class ProviderConfiguration
	{
		public static IServiceCollection AddCustomProviders (this IServiceCollection services)
		{
			services.AddSingleton<IDateTimeProvider, DateTimeProvider> ();
			services.AddSingleton<ITokenProvider, TokenProvider> ();
			services.AddSingleton<IPasswordProvider, PasswordProvider> ();

			return services;
		}
	}
}
