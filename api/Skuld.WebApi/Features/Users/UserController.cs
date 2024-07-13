using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skuld.WebApi.Features.Users.Dto;
using Skuld.WebApi.Infrastructure.Constants;
using Skuld.WebApi.Infrastructure.ErrorHandling;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Skuld.WebApi.Features.Users;

[Authorize (Policy = CustomPolicies.AuthorizedUsersOnly)]
[Produces ("application/json")]
[Consumes ("application/json")]
[Route ("api/[controller]")]
[ApiController]
public class UserController : BaseApiController
{
	private readonly IUserService _userService;

	public UserController (IUserService userService)
	{
		_userService = userService;
	}

	[HttpGet ("me")]
	[ProducesResponseType (StatusCodes.Status200OK, Type = typeof (UserResponse))]
	[ProducesResponseType (StatusCodes.Status404NotFound, Type = typeof (ProblemDetails))]
	[SwaggerResponse (StatusCodes.Status200OK, Type = typeof (UserResponse))]
	[SwaggerResponse (StatusCodes.Status404NotFound, Type = typeof (ProblemDetails))]
	[AllowAnonymous]
	public IActionResult GetUser ()
	{
		var userResult = GetUserIdFromToken ()
			.ContinueWith (EnsureUserId)
			.ContinueWithAsync (_userService.GetUserResultAsync);

		return ToActionResult (userResult);
	}

	private SkuldResult<long> EnsureUserId (long userId)
	{
		return SkuldResult<long>.Error (HttpStatusCode.BadGateway, SkuldErrorType.BadFormatId, "");
	}
}
