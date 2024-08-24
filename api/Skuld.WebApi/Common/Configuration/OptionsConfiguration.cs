using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skuld.WebApi.Common.Configuration.Options;

namespace Skuld.WebApi.Common.Configuration
{
	public static class OptionsConfiguration
	{
		public static IServiceCollection AddCustomOptions (this IServiceCollection services, IConfiguration configuration)
		{
			services.AddOptions ()
					.Configure<JwtOptions> (configuration.GetSection (JwtOptions.SectionName));

			services.AddOptions ()
				.Configure<CORSOptions> (configuration.GetSection (CORSOptions.SectionName));

			return services;
		}
	}
}
