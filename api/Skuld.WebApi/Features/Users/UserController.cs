using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Skuld.WebApi.Common.Constants;
using Skuld.WebApi.Common.ErrorHandling;
using Skuld.WebApi.Features.Users.Dto;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
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

	public UserController (IUserService userService, ProblemDetailsFactory problemDetailsFactory) : base (problemDetailsFactory)
	{
		_userService = userService;
	}

	[HttpGet ("me")]
	[ProducesResponseType (StatusCodes.Status200OK, Type = typeof (UserResponse))]
	[ProducesResponseType (StatusCodes.Status400BadRequest, Type = typeof (ProblemDetails))]
	[ProducesResponseType (StatusCodes.Status401Unauthorized, Type = typeof (ProblemDetails))]
	[ProducesResponseType (StatusCodes.Status403Forbidden, Type = typeof (ProblemDetails))]
	[ProducesResponseType (StatusCodes.Status404NotFound, Type = typeof (ProblemDetails))]
	[ProducesResponseType (StatusCodes.Status500InternalServerError, Type = typeof (ProblemDetails))]
	[SwaggerResponse (StatusCodes.Status200OK, Type = typeof (UserResponse))]
	[SwaggerResponse (StatusCodes.Status400BadRequest, Type = typeof (ProblemDetails))]
	[SwaggerResponse (StatusCodes.Status401Unauthorized, Type = typeof (ProblemDetails))]
	[SwaggerResponse (StatusCodes.Status403Forbidden, Type = typeof (ProblemDetails))]
	[SwaggerResponse (StatusCodes.Status404NotFound, Type = typeof (ProblemDetails))]
	[SwaggerResponse (StatusCodes.Status500InternalServerError, Type = typeof (ProblemDetails))]
	[AllowAnonymous]
	public async Task<IActionResult> GetUser ()
	{
		var userResult = await GetUserIdFromToken ()
			.Then (EnsureUserId)
			.ThenAsync (_userService.GetUserResultAsync);

		return ToActionResult (userResult);
	}

	private SkuldResult<long> EnsureUserId (long userId)
	{
		return SkuldResult<long>.Error (HttpStatusCode.BadGateway, SkuldErrorType.BadFormatId, "");
	}
}
