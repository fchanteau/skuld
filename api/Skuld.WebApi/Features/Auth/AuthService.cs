using AutoMapper;
using Microsoft.Extensions.Logging;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Features.Auth.Dto;
using Skuld.WebApi.Helpers;
using Skuld.WebApi.Infrastructure.ErrorHandling;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Skuld.WebApi.Features.Auth;

public interface IAuthService
{
	Task<SkuldResult<Unit>> AddUserAsync (AddUserPayload payload);
	Task<SkuldResult<TokenInfoResponse>> LoginAsync (LoginPayload payload);
	Task<SkuldResult<string>> ValidRefreshToken (long userId, RefreshTokenPayload payload);
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
		// check if user already exist with the email
		var userAlreadyExist = await UnitOfWork.UserRepository.AnyAsync (x => x.Email.Equals (payload.Email));
		if (userAlreadyExist)
			return SkuldResult<Unit>.Error (HttpStatusCode.BadRequest, SkuldErrorType.UserAlreadyExist, payload.Email);

		var user = _mapper.Map<AddUserPayload, User> (payload);

		// insert user
		UnitOfWork.UserRepository.Insert (user);
		await UnitOfWork.SaveChangesAsync ();


		// create password
		var password = new Password
		{
			Value = _passwordProvider.GenerateCipherPassword (payload.Password),
			UserId = user.UserId,
		};
		UnitOfWork.PasswordRepository.Insert (password);

		// create refresh token for this user
		var refreshToken = _tokenProvider.BuildRefreshToken (user, _dateTimeProvider.UtcNow.AddDays (7));
		UnitOfWork.RefreshTokenRepository.Insert (refreshToken);

		// commit changes
		await UnitOfWork.SaveChangesAsync ();

		return SkuldResult<Unit>.Success (Unit.Instance, HttpStatusCode.Created);
	}

	public async Task<SkuldResult<TokenInfoResponse>> LoginAsync (LoginPayload payload)
	{
		var cryptedPassword = _passwordProvider.GenerateCipherPassword (payload.Password);

		var user = await UnitOfWork.UserRepository.TryGetOneAsync (filter: x => x.Email.Equals (payload.Email), navigationProperties: x => x.Passwords);
		var validPassword = user?.Passwords.Any (x => x.IsActive && x.Value == cryptedPassword) ?? false;
		if (user is null || !validPassword)
		{
			return SkuldResult<TokenInfoResponse>.Error (HttpStatusCode.BadRequest, SkuldErrorType.UserLoginFailed);
		}

		var token = _tokenProvider.CreateToken (user);

		var refreshToken = await UnitOfWork.RefreshTokenRepository.TryGetFirstAsync (
			filter: x => x.UserId == user.UserId,
			orderBy: x => x.OrderByDescending (rt => rt.ExpiredAt));

		if (refreshToken is null)
		{
			return SkuldResult<TokenInfoResponse>.Error (HttpStatusCode.BadRequest, SkuldErrorType.UserLoginFailed);
		}

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



	public async Task<SkuldResult<string>> ValidRefreshToken (long userId, RefreshTokenPayload payload)
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
