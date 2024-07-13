using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Skuld.WebApi.Infrastructure.ErrorHandling;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Skuld.WebApi.Infrastructure.ActionFilters
{
	public class ValidateInputModelAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting (ActionExecutingContext context)
		{
			var model = context.ActionArguments.FirstOrDefault ().Value;

			if (!Validate (model, out var validationResults))
			{
				var problemDetails = new ProblemDetails ()
				{
					Status = 400,
					Title = SkuldErrorType.ValidationFailed.ToString (),
					Detail = string.Join ('|', validationResults.Select (x => x.ErrorMessage)),
					Type = $"https://httpstatuses.com/400",
					Instance = context.HttpContext.Request.Path
				};
				context.Result = new ObjectResult (problemDetails)
				{
					StatusCode = 400
				};
			}
		}

		protected bool Validate<T> (T? obj, out ICollection<ValidationResult> results) where T : class
		{
			results = new List<ValidationResult> ();

			if (obj is null) return false;

			return Validator.TryValidateObject (obj, new ValidationContext (obj), results, true);
		}
	}
}
