using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Skuld.WebApi.Dto.Users;
using Skuld.WebApi.Features.Shared;
using Skuld.WebApi.Infrastructure.ActionFilters;
using Skuld.WebApi.Infrastructure.Constants;
using Skuld.WebApi.Infrastructure.Exceptions;
using Skuld.WebApi.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Skuld.WebApi.Features.Users
{
	[Authorize (Policy = CustomPolicies.AuthorizedUsersOnly)]
	[Produces ("application/json")]
	[Consumes ("application/json")]
	[Route ("api/[controller]")]
	[ApiController]
	public class UsersController : BaseApiController
	{
		private readonly UserService _userService;

		public UsersController (UserService userService)
		{
			_userService = userService;
		}

		[AllowAnonymous]
		[HttpPost]
		[ProducesResponseType (StatusCodes.Status201Created, Type = typeof (UserResponse))]
		[ProducesResponseType (StatusCodes.Status400BadRequest, Type = typeof (SkuldProblemDetails))]
		[SwaggerResponse (StatusCodes.Status201Created, Type = typeof (UserResponse))]
		[SwaggerResponse (StatusCodes.Status400BadRequest, Type = typeof (SkuldProblemDetails))]
		[ValidateInputModel]
		public async Task<IActionResult> CreateUser ([FromBody] AddUserPayload payload)
		{
			var user = await _userService.AddUserAsync (payload);

			return CreatedAtAction (nameof (GetUser), user);
		}

		[AllowAnonymous]
		[HttpPost ("login")]
		[ProducesResponseType (StatusCodes.Status200OK, Type = typeof (TokenInfoResponse))]
		[ProducesResponseType (StatusCodes.Status400BadRequest, Type = typeof (SkuldProblemDetails))]
		[SwaggerResponse (StatusCodes.Status200OK, Type = typeof (TokenInfoResponse))]
		[SwaggerResponse (StatusCodes.Status400BadRequest, Type = typeof (SkuldProblemDetails))]
		[ValidateInputModel]
		public async Task<IActionResult> Login ([FromBody] LoginPayload payload)
		{
			var result = await _userService.LoginAsync (payload);

			return Ok (result);
		}

		[HttpPost ("refreshtoken")]
		[ProducesResponseType (StatusCodes.Status200OK, Type = typeof (TokenInfoResponse))]
		[ProducesResponseType (StatusCodes.Status400BadRequest, Type = typeof (SkuldProblemDetails))]
		[SwaggerResponse (StatusCodes.Status200OK, Type = typeof (TokenInfoResponse))]
		[SwaggerResponse (StatusCodes.Status400BadRequest, Type = typeof (SkuldProblemDetails))]
		[ValidateInputModel]
		public async Task<IActionResult> RefreshToken ([FromBody] RefreshTokenPayload payload)
		{
			var userId = GetUserIdFromToken ();

			var newToken = await _userService.ValidRefreshToken (userId, payload);

			return Ok (new TokenInfoResponse
			{
				Token = newToken,
				RefreshToken = payload.RefreshToken
			});
		}

		[HttpGet ("me")]
		[ProducesResponseType (StatusCodes.Status200OK, Type = typeof (UserResponse))]
		[ProducesResponseType (StatusCodes.Status404NotFound, Type = typeof (SkuldProblemDetails))]
		[SwaggerResponse (StatusCodes.Status200OK, Type = typeof (UserResponse))]
		[SwaggerResponse (StatusCodes.Status404NotFound, Type = typeof (SkuldProblemDetails))]
		public async Task<IActionResult> GetUser ()
		{
			var user = await _userService.GetUserAsync (GetUserIdFromToken ());

			return Ok (user);
		}

		[HttpPatch ("{userId}")]
		[ProducesResponseType (StatusCodes.Status204NoContent, Type = typeof (void))]
		[ProducesResponseType (StatusCodes.Status400BadRequest, Type = typeof (SkuldProblemDetails))]
		[ProducesResponseType (StatusCodes.Status404NotFound, Type = typeof (SkuldProblemDetails))]
		[SwaggerResponse (StatusCodes.Status204NoContent, Type = typeof (void))]
		[SwaggerResponse (StatusCodes.Status400BadRequest, Type = typeof (SkuldProblemDetails))]
		[SwaggerResponse (StatusCodes.Status404NotFound, Type = typeof (SkuldProblemDetails))]

		public async Task<IActionResult> PatchUser (long userId, [FromBody] JsonPatchDocument<UserResponse> patch)
		{
			var existedUser = await _userService.GetUserAsync (userId);

			patch.ApplyTo (existedUser);

			_userService.UpdateUser (existedUser);

			return NoContent ();
		}
	}
}
