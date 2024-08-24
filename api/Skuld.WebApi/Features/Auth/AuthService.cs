using AutoMapper;
using Microsoft.Extensions.Logging;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Common.ErrorHandling;
using Skuld.WebApi.Features.Auth.Dto;
using Skuld.WebApi.Helpers;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Skuld.WebApi.Features.Auth;

public interface IAuthService
{
	Task<SkuldResult<Unit>> AddUserAsync (AddUserPayload payload);
	Task<SkuldResult<TokenInfoResponse>> LoginAsync (LoginPayload payload);
	Task<SkuldResult<string>> ValidRefreshTokenAsync (long userId, RefreshTokenPayload payload);
}

public class AuthService : BaseService, IAuthService
{
	private readonly IMapper _mapper;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ITokenProvider _tokenProvider;
	private readonly IPasswordProvider _passwordProvider;
	private readonly ILogger<AuthService> _logger;

	#region Constructor

	public AuthService (IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, ITokenProvider tokenProvider, IPasswordProvider passwordProvider, ILogger<AuthService> logger) : base (unitOfWork)
	{
		var config = new MapperConfiguration (cfg =>
		{
			cfg.AddProfile<AuthProfile> ();
		});

		config.AssertConfigurationIsValid ();

		_mapper = new Mapper (config);
		_dateTimeProvider = dateTimeProvider;
		_tokenProvider = tokenProvider;
		_passwordProvider = passwordProvider;
		_logger = logger;
	}

	#endregion

	#region Public methods

	public async Task<SkuldResult<Unit>> AddUserAsync (AddUserPayload payload)
	{
		return await SkuldResult<bool>.Success (await UnitOfWork.UserRepository.AnyAsync (x => x.Email.Equals (payload.Email)))
		.Then (userAlreadyExist =>
		{
			if (userAlreadyExist)
				return SkuldResult<User>.Error (HttpStatusCode.BadRequest, SkuldErrorType.UserAlreadyExist, payload.Email);

			// Map the payload to a User entity
			var user = _mapper.Map<AddUserPayload, User> (payload);
			UnitOfWork.UserRepository.Insert (user);
			return SkuldResult<User>.Success (user);
		})
		// Save the user changes
		.ThenAsync (async user =>
		{
			await UnitOfWork.SaveChangesAsync ();
			return SkuldResult<User>.Success (user);
		})
		// Create and insert the password
		.Then (user =>
		{
			var password = new Password
			{
				Value = _passwordProvider.GenerateCipherPassword (payload.Password),
				UserId = user.UserId,
			};
			UnitOfWork.PasswordRepository.Insert (password);
			return SkuldResult<User>.Success (user);
		})
		// Create and insert the refresh token
		.Then (user =>
		{
			var refreshToken = _tokenProvider.BuildRefreshToken (user, _dateTimeProvider.UtcNow.AddDays (7));
			UnitOfWork.RefreshTokenRepository.Insert (refreshToken);
			return SkuldResult<Unit>.Success (Unit.Instance);
		})
		// Commit all changes
		.ThenAsync (async _ =>
		{
			await UnitOfWork.SaveChangesAsync ();
			return SkuldResult<Unit>.Success (Unit.Instance, HttpStatusCode.Created);
		});
	}

	public async Task<SkuldResult<TokenInfoResponse>> LoginAsync (LoginPayload payload)
	{
		async Task<SkuldResult<User>> ValidateLogin (LoginPayload loginPayload)
		{
			var cryptedPassword = _passwordProvider.GenerateCipherPassword (payload.Password);

			var user = await UnitOfWork.UserRepository.TryGetOneAsync (filter: x => x.Email.Equals (payload.Email), navigationProperties: x => x.Passwords);
			var validPassword = user?.Passwords.Any (x => x.IsActive && x.Value == cryptedPassword) ?? false;
			if (user is null || !validPassword)
			{
				return SkuldResult<User>.Error (HttpStatusCode.BadRequest, SkuldErrorType.UserLoginFailed);
			}
			return SkuldResult<User>.Success (user);
		}

		async Task<SkuldResult<TokenInfoResponse>> ManageTokens (User user)
		{
			var token = _tokenProvider.CreateToken (user);

			var refreshToken = await UnitOfWork.RefreshTokenRepository.TryGetFirstAsync (
				filter: x => x.UserId == user.UserId,
				orderBy: x => x.OrderByDescending (rt => rt.ExpiredAt));

			if (refreshToken is null)
			{
				return SkuldResult<TokenInfoResponse>.Error (HttpStatusCode.BadRequest, SkuldErrorType.UserLoginFailed);
			}

			// Insert new refresh token if current one is expired
			if (refreshToken.ExpiredAt < _dateTimeProvider.UtcNow)
			{
				refreshToken = _tokenProvider.BuildRefreshToken (user, _dateTimeProvider.UtcNow.AddDays (7));
				UnitOfWork.RefreshTokenRepository.Insert (refreshToken);
				await UnitOfWork.SaveChangesAsync ();
			}

			return SkuldResult<TokenInfoResponse>.Success (new TokenInfoResponse
			{
				Token = token,
				RefreshToken = refreshToken.Value
			});
		}

		return await ValidateLogin (payload)
			.ThenAsync (ManageTokens);
	}

	public async Task<SkuldResult<string>> ValidRefreshTokenAsync (long userId, RefreshTokenPayload payload)
	{
		var response = await UnitOfWork.RefreshTokenRepository.TryGetOneAsync (filter: x => x.Value == payload.RefreshToken && x.UserId == userId && x.ExpiredAt > _dateTimeProvider.UtcNow);
		if (response is null)
		{
			return SkuldResult<string>.Error (HttpStatusCode.BadRequest, SkuldErrorType.RefreshTokenInvalid);
		}

		var user = await UnitOfWork.UserRepository.TryGetByIdAsync (userId);

		if (user is null)
		{
			// Should never happen
			return SkuldResult<string>.Error (HttpStatusCode.InternalServerError, SkuldErrorType.UserNotFound);
		}

		return SkuldResult<string>.Success (_tokenProvider.CreateToken (user));
	}

	#endregion
}
