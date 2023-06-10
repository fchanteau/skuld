using Microsoft.AspNetCore.Builder;
using Skuld.WebApi.Infrastructure.Exceptions;

namespace Skuld.WebApi.Infrastructure.Configuration
{
	public static class ExceptionConfiguration
	{
		public static void UseCustomExceptionMiddleware (this IApplicationBuilder app)
		{
			app.UseMiddleware<ExceptionMiddleware> ();
		}
	}
}
