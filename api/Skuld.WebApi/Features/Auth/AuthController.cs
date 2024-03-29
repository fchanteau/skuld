﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skuld.WebApi.Features.Auth.Dto;
using Skuld.WebApi.Infrastructure.ActionFilters;
using Skuld.WebApi.Infrastructure.Constants;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Skuld.WebApi.Features.Auth
{
	[Authorize (Policy = CustomPolicies.AuthorizedUsersOnly)]
	[Produces ("application/json")]
	[Consumes ("application/json")]
	[Route ("api/[controller]")]
	[ApiController]
	public class AuthController : BaseApiController
	{
		private readonly IAuthService _authService;

		public AuthController (IAuthService authService)
		{
			_authService = authService;
		}

		[AllowAnonymous]
		[HttpPost]
		[ProducesResponseType (StatusCodes.Status201Created)]
		[ProducesResponseType (StatusCodes.Status400BadRequest, Type = typeof (ProblemDetails))]
		[SwaggerResponse (StatusCodes.Status201Created)]
		[SwaggerResponse (StatusCodes.Status400BadRequest, Type = typeof (ProblemDetails))]
		[ValidateInputModel]
		public async Task<IActionResult> CreateUser ([FromBody] AddUserPayload payload)
		{
			await _authService.AddUserAsync (payload);

			return StatusCode (201);
		}

		[AllowAnonymous]
		[HttpPost ("login")]
		[ProducesResponseType (StatusCodes.Status200OK, Type = typeof (TokenInfoResponse))]
		[ProducesResponseType (StatusCodes.Status400BadRequest, Type = typeof (ProblemDetails))]
		[SwaggerResponse (StatusCodes.Status200OK, Type = typeof (TokenInfoResponse))]
		[SwaggerResponse (StatusCodes.Status400BadRequest, Type = typeof (ProblemDetails))]
		[ValidateInputModel]
		public async Task<IActionResult> Login ([FromBody] LoginPayload payload)
		{
			var result = await _authService.LoginAsync (payload);

			return Ok (result);
		}

		[HttpPost ("refreshtoken")]
		[ProducesResponseType (StatusCodes.Status200OK, Type = typeof (TokenInfoResponse))]
		[ProducesResponseType (StatusCodes.Status400BadRequest, Type = typeof (ProblemDetails))]
		[SwaggerResponse (StatusCodes.Status200OK, Type = typeof (TokenInfoResponse))]
		[SwaggerResponse (StatusCodes.Status400BadRequest, Type = typeof (ProblemDetails))]
		[ValidateInputModel]
		public async Task<IActionResult> RefreshToken ([FromBody] RefreshTokenPayload payload)
		{
			var userId = GetUserIdFromToken ();

			var newToken = await _authService.ValidRefreshToken (userId, payload);

			return Ok (new TokenInfoResponse
			{
				Token = newToken,
				RefreshToken = payload.RefreshToken
			});
		}
	}
}
