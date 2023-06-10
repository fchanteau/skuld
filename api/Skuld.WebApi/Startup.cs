using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Skuld.Data;
using Skuld.WebApi.Infrastructure.Configuration;

namespace Skuld.WebApi
{
	public class Startup
	{
		public Startup (IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices (IServiceCollection services)
		{
			services.AddDbContext<SkuldContext> (options => options.UseSqlServer (Configuration.GetConnectionString ("Skuld") ?? throw new System.Exception ("No connectrion string found"),
				sqloptions => sqloptions.EnableRetryOnFailure ()));

			services.AddCustomDependencyInjection ();

			services.AddMvc ();
			services.AddControllers ();
			services.AddEndpointsApiExplorer ();

			services.AddCustomSwaggerGen (Configuration);

			services.AddCustomOptions (Configuration);

			services.AddCustomAuthentication (Configuration);

			services.AddCustomAuthorization ();

			services.Configure<ApiBehaviorOptions> (options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseCors (cors =>
				cors.WithOrigins ("http://localhost:3000", "http://localhost:4173")
					.AllowAnyHeader ()
					.AllowAnyMethod ()
					.SetIsOriginAllowedToAllowWildcardSubdomains ()
			);

			if (env.IsDevelopment ())
			{
				app.UseDeveloperExceptionPage ();
			}

			app.UseCustomExceptionMiddleware ();

			app.UseHttpsRedirection ();

			app.UseRouting ();

			app.UseAuthentication ();
			app.UseAuthorization ();

			app.UseEndpoints (endpoints =>
			{
				endpoints.MapControllers ();
			});

			app.UseCustomSwagger (Configuration);
		}
	}
}
