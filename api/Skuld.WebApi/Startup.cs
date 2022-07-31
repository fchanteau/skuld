using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Skuld.Data;
using Skuld.WebApi.Infrastructure.Configuration;
using System.Linq;

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
			services.AddDbContext<SkuldContext> (options => options.UseSqlServer (Configuration.GetConnectionString ("Skuld"),
				sqloptions => sqloptions.EnableRetryOnFailure ()));

			services.AddCustomDependencyInjection ();

			services.AddCustomSwaggerGen (this.Configuration);

			services.AddControllers (options =>
			{
				options.InputFormatters.Insert (0, GetJsonPatchInputFormatter ());
			});

			services.AddCustomOptions (this.Configuration);

			services.AddCustomAuthentication (this.Configuration);

			services.AddCustomAuthorization ();

			services.Configure<ApiBehaviorOptions> (options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

			services.AddRouting (options =>
			{
				options.LowercaseUrls = true;
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

			app.UseCustomSwagger (this.Configuration);
		}

		private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter ()
		{
			var builder = new ServiceCollection ()
				.AddLogging ()
				.AddMvc ()
				.AddNewtonsoftJson ()
				.Services.BuildServiceProvider ();

			return builder
				.GetRequiredService<IOptions<MvcOptions>> ()
				.Value
				.InputFormatters
				.OfType<NewtonsoftJsonPatchInputFormatter> ()
				.First ();
		}
	}
}
