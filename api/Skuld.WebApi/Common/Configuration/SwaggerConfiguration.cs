using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Skuld.WebApi.Common.Configuration
{
	public static class SwaggerConfiguration
	{
		public static IServiceCollection AddCustomSwaggerGen (this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSwaggerGen (c =>
			{
				c.SwaggerDoc ("v1", new OpenApiInfo { Title = "Skuld.WebApi", Version = "v1" });
			});

			return services;
		}

		public static IApplicationBuilder UseCustomSwagger (this IApplicationBuilder app, IConfiguration configuration, string routePrefix = "swagger")
		{
			app.UseSwagger ();
			app.UseSwaggerUI (c => c.SwaggerEndpoint ("/swagger/v1/swagger.json", "Skuld.WebApi v1"));

			return app;
		}
	}
}
