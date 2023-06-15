﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Exceptions;
using Skuld.WebApi.Features.Auth.Dto;
using Skuld.WebApi.Helpers;
using Skuld.WebApi.Infrastructure.Configuration.Options;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Skuld.WebApi.Features.Auth
{
	public interface IAuthService
	{
		Task<UserResponse> AddUserAsync (AddUserPayload payload);
		Task<TokenInfoResponse> LoginAsync (LoginPayload payload);
		Task<UserResponse> GetUserAsync (long userId);
		Task<string> ValidRefreshToken (long userId, RefreshTokenPayload payload);
	}

	public class AuthService : BaseService, IAuthService
	{
		private readonly JwtOptions _jwtOptions;
		private readonly ILogger<AuthService> _logger;
		private readonly IMapper _mapper;

		#region Constructor

		public AuthService (IUnitOfWork unitOfWork, IOptions<JwtOptions> jwtOptions, ILogger<AuthService> logger) : base (unitOfWork)
		{
			var config = new MapperConfiguration (cfg =>
			{
				cfg.AddProfile<AuthProfile> ();
			});

			config.AssertConfigurationIsValid ();

			_mapper = new Mapper (config);
			_jwtOptions = jwtOptions.Value;
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

			var user = _mapper.Map<AddUserPayload, User> (payload);

			// insert user
			UnitOfWork.UserRepository.Insert (user);
			await UnitOfWork.SaveChangesAsync ();


			// create password
			var password = new Password
			{
				Value = Convert.ToBase64String (Encoding.ASCII.GetBytes (payload.Password ?? throw new Exception ())), // TODO FCU : better handling here with generic exception
				UserId = user.UserId,
			};
			UnitOfWork.PasswordRepository.Insert (password);

			// create refresh token for this user
			UnitOfWork.RefreshTokenRepository.Insert (TokenHelper.BuildRefreshToken (user, DateTime.Now.AddDays (7)));

			// commit changes
			await UnitOfWork.SaveChangesAsync ();

			var userCreated = await UnitOfWork.UserRepository.TryGetByIdAsync (user.UserId);

			if (userCreated is null)
			{
				// TODO FCU : better handling here, return custom error ?
				throw new Exception ();
			}

			return _mapper.Map<User, UserResponse> (userCreated);
		}

		public async Task<TokenInfoResponse> LoginAsync (LoginPayload payload)
		{
			var cryptedPassword = Convert.ToBase64String (Encoding.ASCII.GetBytes (payload.Password ?? ""));

			var user = await UnitOfWork.UserRepository.TryGetOneAsync (filter: x => x.Email.Equals (payload.Email), navigationProperties: x => x.Passwords);
			var validPassword = user?.Passwords.Any (x => x.IsActive && x.Value == cryptedPassword) ?? false;
			if (user is null || !validPassword)
			{
				throw new SkuldException (HttpStatusCode.BadRequest, SkuldExceptionType.UserLoginFailed);
			}

			var token = TokenHelper.CreateToken (user, _jwtOptions);

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
			var user = await UnitOfWork.UserRepository.TryGetFirstAsync (user => user.UserId == userId);
			if (user is null)
				throw new SkuldException (HttpStatusCode.NotFound, SkuldExceptionType.UserNotFound);

			return _mapper.Map<User, UserResponse> (user);
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

			// TODO FCU : better handling here
			return TokenHelper.CreateToken (user!, _jwtOptions);
		}

		#endregion
	}
}