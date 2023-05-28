﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Dto.Users;
using Skuld.WebApi.Exceptions;
using Skuld.WebApi.Helpers;
using Skuld.WebApi.Infrastructure.Configuration.Options;
using Skuld.WebApi.MappingProfiles;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Skuld.WebApi.Services
{
	public interface IUserService
	{
		Task<UserResponse> AddUserAsync (AddUserPayload payload);
		Task<TokenInfoResponse> LoginAsync (LoginPayload payload);
		Task<UserResponse> GetUserAsync (long userId);
		Task<string> ValidRefreshToken (long userId, RefreshTokenPayload payload);
		Task<bool> UpdateUserAsync (UserResponse user);
	}

	public class UserService : BaseService, IUserService
	{
		private readonly JwtOptions jwtOptions;
		private readonly ILogger<UserService> _logger;

		#region Constructor

		public UserService (IUnitOfWork unitOfWork, IOptions<JwtOptions> jwtOptions, ILogger<UserService> logger) : base (unitOfWork)
		{
			var config = new MapperConfiguration (cfg =>
			{
				cfg.AddProfile<UserProfile> ();
			});

			config.AssertConfigurationIsValid ();

			Mapper = new Mapper (config);
			this.jwtOptions = jwtOptions.Value;
			_logger = logger;
		}

		#endregion

		#region Public methods

		public async Task<UserResponse> AddUserAsync (AddUserPayload payload)
		{
			// check if user already exist with the email
			var userAlreadyExist = await UnitOfWork.UserRepository.AnyAsync (x => x.Email.Equals (payload.Email));
			if (userAlreadyExist)
				throw new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.UserAlreadyExist, payload.Email);

			var user = Mapper.Map<AddUserPayload, User> (payload);

			// insert user
			UnitOfWork.UserRepository.Insert (user);
			await UnitOfWork.SaveChangesAsync ();


			// create password
			var password = new Password
			{
				Value = Convert.ToBase64String (Encoding.ASCII.GetBytes (payload.Password)),
				UserId = user.UserId,
			};
			UnitOfWork.PasswordRepository.Insert (password);

			// create refresh token for this user
			UnitOfWork.RefreshTokenRepository.Insert (TokenHelper.BuildRefreshToken (user, DateTime.Now.AddDays (7)));

			// commit changes
			await UnitOfWork.SaveChangesAsync ();

			var userCreated = await UnitOfWork.UserRepository.TryGetByIdAsync (user.UserId);

			return Mapper.Map<User, UserResponse> (userCreated);
		}

		public async Task<TokenInfoResponse> LoginAsync (LoginPayload payload)
		{
			var cryptedPassword = Convert.ToBase64String (Encoding.ASCII.GetBytes (payload.Password));

			var user = await UnitOfWork.UserRepository.TryGetOneAsync (filter: x => x.Email.Equals (payload.Email), navigationProperties: x => x.Passwords);
			var validPassword = user?.Passwords.Any (x => x.IsActive && x.Value.Equals (cryptedPassword)) ?? false;
			if (user is null || !validPassword)
			{
				throw new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.UserLoginFailed);
			}

			var token = TokenHelper.CreateToken (user, jwtOptions);

			var refreshToken = await UnitOfWork.RefreshTokenRepository.TryGetFirstAsync (
				filter: x => x.UserId == user.UserId,
				orderBy: x => x.OrderByDescending (rt => rt.ExpiredAt));

			if (refreshToken is null)
			{
				throw new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.UserLoginFailed);
			}

			if (refreshToken.ExpiredAt < DateTime.Now)
			{
				refreshToken = TokenHelper.BuildRefreshToken (user, DateTime.Now.AddDays (7));
				UnitOfWork.RefreshTokenRepository.Insert (refreshToken);
				await UnitOfWork.SaveChangesAsync ();
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
			var user = await UnitOfWork.UserRepository.TryGetFirstAsync (user => user.UserId == userId);
			if (user is null)
				throw new SkuldException (HttpStatusCode.NotFound, SkuldExceptionType.UserNotFound);

			return Mapper.Map<User, UserResponse> (user);
		}

		public async Task<string> ValidRefreshToken (long userId, RefreshTokenPayload payload)
		{
			var response = await UnitOfWork.RefreshTokenRepository.TryGetOneAsync (filter: x => x.Value == payload.RefreshToken && x.UserId == userId);
			if (response is null ||
				response.ExpiredAt < DateTime.Now)
			{
				throw new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.RefreshTokenInvalid);
			}

			var user = await UnitOfWork.UserRepository.TryGetByIdAsync (userId);

			return TokenHelper.CreateToken (user, jwtOptions);
		}

		public async Task<bool> UpdateUserAsync (UserResponse user)
		{
			try
			{
				UnitOfWork.UserRepository.Update (
				Mapper.Map<UserResponse, User> (user));

				return await UnitOfWork.SaveChangesAsync () > 0;
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
