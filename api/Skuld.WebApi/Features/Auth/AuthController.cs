﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Skuld.WebApi.Common.ActionFilters;
using Skuld.WebApi.Common.Constants;
using Skuld.WebApi.Common.ErrorHandling;
using Skuld.WebApi.Features.Auth.Dto;
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

		public AuthController (IAuthService authService, ProblemDetailsFactory problemDetailsFactory) : base (problemDetailsFactory)
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
			var result = await _authService.AddUserAsync (payload);

			return ToActionResult (result);
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

			return ToActionResult (result);
		}

		[HttpPost ("refreshtoken")]
		[ProducesResponseType (StatusCodes.Status200OK, Type = typeof (TokenInfoResponse))]
		[ProducesResponseType (StatusCodes.Status400BadRequest, Type = typeof (ProblemDetails))]
		[SwaggerResponse (StatusCodes.Status200OK, Type = typeof (TokenInfoResponse))]
		[SwaggerResponse (StatusCodes.Status400BadRequest, Type = typeof (ProblemDetails))]
		[ValidateInputModel]
		public async Task<IActionResult> RefreshToken ([FromBody] RefreshTokenPayload payload)
		{
			var result = await GetUserIdFromToken ()
				.ThenAsync (userId => _authService.ValidRefreshToken (userId, payload))
				.Then (newToken => SkuldResult<TokenInfoResponse>.Success (new TokenInfoResponse
				{
					Token = newToken,
					RefreshToken = payload.RefreshToken
				}));

			return ToActionResult (result);
		}
	}
}
