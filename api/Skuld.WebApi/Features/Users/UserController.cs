using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skuld.WebApi.Features.Users.Dto;
using Skuld.WebApi.Infrastructure.Constants;
using Skuld.WebApi.Infrastructure.ErrorHandling;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

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
	public async Task<IActionResult> GetUser ()
	{
		var user = await _userService.GetUserAsync (100);

		return Ok (user);
	}

	[HttpGet ("me/result")]
	[ProducesResponseType (StatusCodes.Status200OK, Type = typeof (UserResponse))]
	[ProducesResponseType (StatusCodes.Status404NotFound, Type = typeof (ProblemDetails))]
	[SwaggerResponse (StatusCodes.Status200OK, Type = typeof (UserResponse))]
	[SwaggerResponse (StatusCodes.Status404NotFound, Type = typeof (ProblemDetails))]
	[AllowAnonymous]
	public IActionResult GetUserResult ()
	{
		var userResult = GetUserIdFromTokenResult ()
			.ContinueWith (EnsureUserId)
			.ContinueWithAsync (userId => _userService.GetUserResultAsync (userId));

		return ToActionResult (userResult);
	}

	private SkuldResult<long> EnsureUserId (long userId)
	{
		return SkuldResult<long>.Success (userId);
	}
}
