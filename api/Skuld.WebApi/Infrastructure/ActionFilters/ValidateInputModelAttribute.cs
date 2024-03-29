﻿using Microsoft.AspNetCore.Mvc.Filters;
using Skuld.WebApi.Exceptions;
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

			// TODO FCU : check this exception and maybe create SkuldExceptionType
			if (model is null)
			{
				throw new SkuldException (System.Net.HttpStatusCode.BadRequest, SkuldExceptionType.ValidationFailed);
			}

			if (!Validate (model, out var validationResults))
			{
				throw new SkuldException (System.Net.HttpStatusCode.BadRequest, SkuldExceptionType.ValidationFailed, validationResults.Select (x => x.ErrorMessage).ToArray ());
			}
		}

		protected bool Validate<T> (T obj, out ICollection<ValidationResult> results) where T : notnull
		{
			results = new List<ValidationResult> ();

			return Validator.TryValidateObject (obj, new ValidationContext (obj), results, true);
		}
	}
}
