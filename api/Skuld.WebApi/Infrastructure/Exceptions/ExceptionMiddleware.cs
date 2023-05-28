using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Skuld.WebApi.Exceptions;
using Skuld.WebApi.Ressources;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Skuld.WebApi.Infrastructure.Exceptions
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IWebHostEnvironment _hostingEnvironment;

		public ExceptionMiddleware (RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment hostingEnvironment)
		{
			_next = next;
			_logger = logger;
			_hostingEnvironment = hostingEnvironment;
		}

		public async Task InvokeAsync (HttpContext httpContext)
		{
			try
			{
				await _next (httpContext);
			}
			catch (SkuldException ex)
			{
				await HandleSkuldException (httpContext, ex);
			}
			catch (JsonPatchException ex)
			{
				await HandleSkuldException (httpContext, new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.JsonPatchException, new string[] { ex.Message }));
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync (httpContext, ex);
			}
		}

		private Task HandleSkuldException (HttpContext httpContext, SkuldException ex)
		{
			httpContext.Response.ContentType = "application/json";
			httpContext.Response.StatusCode = (int)ex.HttpStatusCode;

			string? message;
			if (ex.Parameters is null)
			{
				message = ErrorMessage.ResourceManager.GetString (ex.SkuldExceptionType.ToString ());
			}
			else if (ex.SkuldExceptionType == SkuldExceptionType.ValidationFailed)
			{
				// TODO FCU : better handling here
				message = string.Format (ErrorMessage.ResourceManager.GetString (ex.SkuldExceptionType.ToString ()) ?? throw new Exception (), string.Join (" | ", ex.Parameters));
			}
			else
			{
				message = string.Format (ErrorMessage.ResourceManager.GetString (ex.SkuldExceptionType.ToString ()) ?? throw new Exception (), ex.Parameters);
			}

			_logger.LogError (message);
			return httpContext.Response.WriteAsync (new SkuldProblemDetails ()
			{
				Status = httpContext.Response.StatusCode,
				Title = ex.SkuldExceptionType.ToString (),
				Detail = message,
				Type = $"https://httpstatuses.com/{httpContext.Response.StatusCode}",
				Instance = httpContext.Request.Path
			}.ToString ());

		}

		private Task HandleExceptionAsync (HttpContext httpContext, Exception exception)
		{
			httpContext.Response.ContentType = "application/json";
			httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var message = _hostingEnvironment.IsDevelopment () ? $"{exception.Message} {exception.StackTrace}" : "Internal Server Error not handled by the system.";

			_logger.LogError (message);

			return httpContext.Response.WriteAsync (new SkuldProblemDetails ()
			{
				Status = httpContext.Response.StatusCode,
				Title = "InternalServerError",
				Type = "https://httpstatuses.com/500",
				Detail = message,
				Instance = httpContext.Request.Path
			}.ToString ());
		}
	}

	public class SkuldProblemDetails : ProblemDetails
	{
		public override string ToString ()
		{
			return JsonConvert.SerializeObject (this);
		}
	}
}
