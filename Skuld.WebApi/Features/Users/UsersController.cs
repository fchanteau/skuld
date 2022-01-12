using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skuld.Shared.DTO.Users;
using Skuld.Shared.Infrastructure.Constants;
using Skuld.Shared.Services;
using Skuld.WebApi.Features.Shared;
using Skuld.WebApi.Features.Users.Models;
using Skuld.WebApi.Infrastructure.ActionFilters;
using Skuld.WebApi.Infrastructure.Exceptions;
using Skuld.WebApi.Infrastructure.MappingProfile;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Skuld.Shared.Helpers;

namespace Skuld.WebApi.Features.Users
{
    [Authorize(Policy = CustomPolicies.AuthorizedUsersOnly)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });

            config.AssertConfigurationIsValid();
            this._mapper = new Mapper(config);
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserGetModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SkuldProblemDetails))]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(UserGetModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(SkuldProblemDetails))]
        [ValidateInputModel]
        public async Task<IActionResult> CreateUser([FromBody] UserPostModel model)
        {
            var modelConverted = this._mapper.Map<UserPostModel, CreateUserDTO>(model);
            var user = await this._userService.CreateUserAsync(modelConverted);

            return CreatedAtAction(nameof(GetUser), this._mapper.Map<UserDTO, UserGetModel>(user));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SkuldProblemDetails))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TokenResponseModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(SkuldProblemDetails))]
        [ValidateInputModel]
        public async Task<IActionResult> Login([FromBody] UserPostLoginModel model)
        {
            var modelConverted = this._mapper.Map<UserPostLoginModel, UserLoginDTO>(model);
            var result = await this._userService.LoginAsync(modelConverted);

            return Ok(new TokenResponseModel(result.Item1, result.Item2));
        }

        [HttpPost("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SkuldProblemDetails))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TokenResponseModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(SkuldProblemDetails))]
        [ValidateInputModel]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenPostModel model)
        {
            var userId = this.GetUserIdFromToken();

            var newToken = await this._userService.ValidRefreshToken(userId, model.RefreshToken);

            return Ok(new TokenResponseModel(newToken, model.RefreshToken));
        }

        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserGetModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(SkuldProblemDetails))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserGetModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(SkuldProblemDetails))]
        public async Task<IActionResult> GetUser()
        {
            var user = await this._userService.GetUserAsync(this.GetUserIdFromToken());

            var response = this._mapper.Map<UserDTO, UserGetModel>(user);

            return Ok(response);
        }
    }
}
