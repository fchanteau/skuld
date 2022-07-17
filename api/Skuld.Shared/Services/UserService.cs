using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.Shared.Dto.Users;
using Skuld.Shared.Exceptions;
using Skuld.Shared.Helpers;
using Skuld.Shared.Infrastructure.Configuration.Options;
using Skuld.Shared.MappingProfiles;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Skuld.Shared.Services
{
	public class UserService : BaseService
	{
		private readonly JwtOptions jwtOptions;
		private readonly ILogger<UserService> _logger;

		#region Constructor

		public UserService (UnitOfWork unitOfWork, IOptions<JwtOptions> jwtOptions, ILogger<UserService> logger) : base (unitOfWork)
		{
			var config = new MapperConfiguration (cfg =>
			{
				cfg.AddProfile<UserProfile> ();
			});

			config.AssertConfigurationIsValid ();

			this.Mapper = new Mapper (config);
			this.jwtOptions = jwtOptions.Value;
			this._logger = logger;
		}

		#endregion

		#region Public methods

		public async Task<UserResponse> AddUserAsync (AddUserPayload payload)
		{
			// check if user already exist with the email
			var userAlreadyExist = await this._unitOfWork.UserRepository.AnyAsync (x => x.Email.Equals (payload.Email));
			if (userAlreadyExist)
				throw new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.UserAlreadyExist, payload.Email);

			var user = this.Mapper.Map<AddUserPayload, User> (payload);

			// insert user
			this._unitOfWork.UserRepository.Insert (user);
			this._unitOfWork.Save ();


			// create password
			var password = new Password
			{
				Value = Convert.ToBase64String (Encoding.ASCII.GetBytes (payload.Password)),
				UserId = user.UserId,
			};
			this._unitOfWork.PasswordRepository.Insert (password);

			// create refresh token for this user
			this._unitOfWork.RefreshTokenRepository.Insert (TokenHelper.BuildRefreshToken (user, DateTime.Now.AddDays (7)));

			// commit changes
			this._unitOfWork.Save ();

			var userCreated = await this._unitOfWork.UserRepository.TryGetByIdAsync (user.UserId);

			return this.Mapper.Map<User, UserResponse> (userCreated);
		}

		public async Task<TokenInfoResponse> LoginAsync (LoginPayload payload)
		{
			var cryptedPassword = Convert.ToBase64String (Encoding.ASCII.GetBytes (payload.Password)); // secure password

			var user = await this._unitOfWork.UserRepository.TryGetOneAsync (filter: x => x.Email.Equals (payload.Email), navigationProperties: x => x.Passwords);
			var validPassword = user?.Passwords.Any (x => x.IsActive && x.Value.Equals (cryptedPassword)) ?? false;
			if (user is null || !validPassword)
			{
				throw new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.UserLoginFailed);
			}

			string token = TokenHelper.CreateToken (user, this.jwtOptions);

			var refreshToken = await this._unitOfWork.RefreshTokenRepository.TryGetFirstAsync (
				filter: x => x.UserId == user.UserId,
				orderBy: x => x.OrderByDescending (rt => rt.ExpiredAt));

			if (refreshToken is null)
			{
				throw new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.UserLoginFailed);
			}

			if (refreshToken.ExpiredAt < DateTime.Now)
			{
				refreshToken = TokenHelper.BuildRefreshToken (user, DateTime.Now.AddDays (7));
				this._unitOfWork.RefreshTokenRepository.Insert (refreshToken);
				this._unitOfWork.Save ();
			}

			return new TokenInfoResponse
			{
				Token = token,
				RefreshToken = refreshToken.Value
			};
		}

		public async Task<UserResponse> GetUserAsync (long userId)
		{
			//var user = await this._unitOfWork.UserRepository.TryGetByIdAsync(userId);
			var user = await this._unitOfWork.UserRepository.TryGetFirstAsync (user => user.UserId == userId);
			if (user is null)
				throw new SkuldException (HttpStatusCode.NotFound, SkuldExceptionType.UserNotFound);

			return this.Mapper.Map<User, UserResponse> (user);
		}

		public async Task<string> ValidRefreshToken (long userId, RefreshTokenPayload payload)
		{
			var response = await this._unitOfWork.RefreshTokenRepository.TryGetOneAsync (filter: x => x.Value == payload.RefreshToken && x.UserId == userId);
			if (response is null ||
				response.ExpiredAt < DateTime.Now)
			{
				throw new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.RefreshTokenInvalid);
			}

			var user = await this._unitOfWork.UserRepository.TryGetByIdAsync (userId);

			return TokenHelper.CreateToken (user, jwtOptions);
		}

		public bool UpdateUser (UserResponse user)
		{
			try
			{
				this._unitOfWork.UserRepository.Update (
				Mapper.Map<UserResponse, User> (user));

				return this._unitOfWork.Save () > 0;
			}
			catch (Exception ex)
			{
				_logger.LogError ($"Error while updating user {user.UserId} : {ex.Message}");
				throw new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.UserUpdateFailed);
			}

		}

		#endregion
	}
}
