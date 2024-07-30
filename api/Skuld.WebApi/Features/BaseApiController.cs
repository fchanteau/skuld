using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Skuld.WebApi.Infrastructure.Constants;
using Skuld.WebApi.Infrastructure.ErrorHandling;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Skuld.WebApi.Features;

public abstract class BaseApiController : ControllerBase
{
	private readonly ProblemDetailsFactory _problemDetailsFactory;

	protected BaseApiController (ProblemDetailsFactory problemDetailsFactory) : base ()
	{
		_problemDetailsFactory = problemDetailsFactory;
	}

	protected SkuldResult<long> GetUserIdFromToken ()
	{
		var userIdClaim = User.Claims.FirstOrDefault (x => x.Type.Equals (CustomClaimTypes.UserId));

		if (long.TryParse (userIdClaim?.Value, out var id))
		{
			return SkuldResult<long>.Success (id);
		}

		return SkuldResult<long>.Error (HttpStatusCode.InternalServerError, SkuldErrorType.RefreshTokenInvalid, "");
	}

	protected Task<string?> GetAccessTokenAsync ()
	{
		return HttpContext.GetTokenAsync ("access_token");
	}

	protected IActionResult ToActionResult<T> (SkuldResult<T> skuldResult)
	{
		return skuldResult.Match (ToSuccessActionResult, ToErrorActionResult);
	}

	private IActionResult ToErrorActionResult (HttpStatusCode httpStatusCode, SkuldErrorType skuldExceptionType, params object?[] parameters)
	{
		string? message;

		if (parameters.Any ())
		{
			message = "message with string format";
		}
		else
		{
			message = "message wiothout string format";
		}

		var problemDetails = _problemDetailsFactory.CreateProblemDetails (
			HttpContext,
			statusCode: (int)httpStatusCode,
			title: skuldExceptionType.ToString (),
			detail: message);

		return new ObjectResult (problemDetails)
		{
			StatusCode = (int)httpStatusCode
		};
	}

	private IActionResult ToSuccessActionResult<T> (HttpStatusCode httpStatusCode, T data)
	{
		return httpStatusCode switch
		{
			HttpStatusCode.OK => Ok (data),
			HttpStatusCode.Created => StatusCode (201),
			HttpStatusCode.NoContent => NoContent (),
			_ => throw new ArgumentException ("HTTP status code not handled", nameof (httpStatusCode))
		};
	}
}
