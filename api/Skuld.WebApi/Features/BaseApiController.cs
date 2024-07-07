using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Skuld.WebApi.Exceptions;
using Skuld.WebApi.Infrastructure.Constants;
using Skuld.WebApi.Infrastructure.ErrorHandling;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Skuld.WebApi.Features;

public abstract class BaseApiController : ControllerBase
{
	protected BaseApiController () : base ()
	{
	}

	protected long GetUserIdFromToken ()
	{
		var userIdClaim = User.Claims.FirstOrDefault (x => x.Type.Equals (CustomClaimTypes.UserId));

		// TODO FCU : Create a SkuldExceptionType for this below
		if (userIdClaim == null)
			throw new SkuldException (System.Net.HttpStatusCode.InternalServerError, SkuldExceptionType.RefreshTokenInvalid, "");

		return long.TryParse (userIdClaim.Value, out var id) ? id : 0;
	}

	protected Task<string?> GetAccessTokenAsync ()
	{
		return HttpContext.GetTokenAsync ("access_token");
	}

	protected IActionResult ToActionResult<T> (SkuldResult<T> skuldResult)
	{
		return skuldResult.Match (ToSuccessActionResult, ToErrorActionResult);
	}

	private IActionResult ToErrorActionResult (HttpStatusCode httpStatusCode, SkuldExceptionType skuldExceptionType, params object[] parameters)
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

		return Problem (message,
			HttpContext.Request.Path,
			(int)httpStatusCode,
			skuldExceptionType.ToString (),
			$"https://httpstatuses.com/{httpStatusCode}");
	}

	private IActionResult ToSuccessActionResult<T> (HttpStatusCode httpStatusCode, T data)
	{
		return httpStatusCode switch
		{
			HttpStatusCode.OK => Ok (data),
			HttpStatusCode.NoContent => NoContent (),
			_ => throw new ArgumentException ("HTTP status code not handled", nameof (httpStatusCode))
		};
	}
}
