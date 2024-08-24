using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Common.ErrorHandling;
using Skuld.WebApi.Features.Auth;
using Skuld.WebApi.Features.Auth.Dto;
using Skuld.WebApi.Helpers;
using System.Linq.Expressions;
using System.Net;

namespace Skuld.WebApi.Tests.Features.Auth;

public class AuthServiceTests
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IGenericRepository<User> _userRepository;
	private readonly IGenericRepository<Password> _passwordRepository;
	private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ITokenProvider _tokenProvider;
	private readonly IPasswordProvider _passwordProvider;

	private readonly AuthService _authService;

	public AuthServiceTests ()
	{
		_unitOfWork = Substitute.For<IUnitOfWork> ();
		_userRepository = Substitute.For<IGenericRepository<User>> ();
		_passwordRepository = Substitute.For<IGenericRepository<Password>> ();
		_refreshTokenRepository = Substitute.For<IGenericRepository<RefreshToken>> ();
		_dateTimeProvider = Substitute.For<IDateTimeProvider> ();
		_tokenProvider = Substitute.For<ITokenProvider> ();
		_passwordProvider = Substitute.For<IPasswordProvider> ();

		_unitOfWork.UserRepository.Returns (_userRepository);
		_unitOfWork.PasswordRepository.Returns (_passwordRepository);
		_unitOfWork.RefreshTokenRepository.Returns (_refreshTokenRepository);

		_authService = new AuthService (_unitOfWork, _dateTimeProvider, _tokenProvider, _passwordProvider, NullLogger<AuthService>.Instance);
	}

	[Fact]
	public async Task AddUserAsync_Should_Return_UserCreated ()
	{
		// Given
		var payload = new AddUserPayload
		{
			Email = "Test",
			FirstName = "Test",
			LastName = "Test",
			Password = "Test"
		};
		_userRepository.AnyAsync (filter: Arg.Any<Expression<Func<User, bool>>> ()).Returns (false);
		_unitOfWork.SaveChangesAsync ().Returns (1, 2);
		_passwordProvider.GenerateCipherPassword (Arg.Any<string> ()).Returns ("TestCipher");
		_dateTimeProvider.UtcNow.Returns (new DateTime (2023, 6, 14));
		_tokenProvider.BuildRefreshToken (Arg.Any<User> (), Arg.Any<DateTime> ()).Returns (new RefreshToken ());

		// When
		await _authService.AddUserAsync (payload);

		// Then
		await _userRepository.ReceivedWithAnyArgs (1).AnyAsync (filter: default);
		await _unitOfWork.Received (2).SaveChangesAsync ();
		_passwordProvider.ReceivedWithAnyArgs (1).GenerateCipherPassword (default);
		_passwordRepository.ReceivedWithAnyArgs (1).Insert (default!);
		_tokenProvider.ReceivedWithAnyArgs (1).BuildRefreshToken (default!, default);
		_refreshTokenRepository.ReceivedWithAnyArgs (1).Insert (default!);
	}

	[Fact]
	public async Task AddUserAsync_Should_Throw_SkuldException_UserAlreadyExist ()
	{
		// Given
		_userRepository.AnyAsync (filter: Arg.Any<Expression<Func<User, bool>>> ()).Returns (true);

		// When
		var action = () => _authService.AddUserAsync (new AddUserPayload ());

		// Then
		await action.Should ().ThrowAsync<SkuldException> ()
			.Where (ex => ex.SkuldExceptionType == SkuldErrorType.UserAlreadyExist && ex.HttpStatusCode == HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task LoginAsync_Should_Return_TokenInfoResponse ()
	{
		// Given
		var userDb = new User
		{
			UserId = 1,
			Email = "test@test.com",
			Passwords = new List<Password>
			{
				new ()
				{
					Value = "TestCipher",
					PasswordId = 1,
					IsActive = true,
				}
			}
		};
		var refreshTokenDB = new RefreshToken
		{
			RefreshTokenId = 1,
			Value = "refreshToken",
			UserId = 1,
			ExpiredAt = new DateTime (2024, 6, 14)
		};
		var payload = new LoginPayload
		{
			Email = userDb.Email,
			Password = "password"
		};
		var tokenInfoResponse = new TokenInfoResponse
		{
			Token = "token",
			RefreshToken = "refreshToken"
		};
		_passwordProvider.GenerateCipherPassword (payload.Password).Returns ("TestCipher");
		_userRepository.TryGetOneAsync (filter: Arg.Any<Expression<Func<User, bool>>> (), navigationProperties: Arg.Any<Expression<Func<User, object>>[]> ()).Returns (userDb);
		_tokenProvider.CreateToken (Arg.Any<User> ()).Returns ("token");
		_refreshTokenRepository
			.TryGetFirstAsync (
				filter: Arg.Any<Expression<Func<RefreshToken, bool>>> (),
				orderBy: Arg.Any<Func<IQueryable<RefreshToken>, IOrderedQueryable<RefreshToken>>> ())
			.Returns (refreshTokenDB);
		_dateTimeProvider.UtcNow.Returns (new DateTime (2023, 6, 14));

		// When
		var result = await _authService.LoginAsync (payload);

		// Then
		result.Should ().BeEquivalentTo (tokenInfoResponse);
		_passwordProvider.ReceivedWithAnyArgs (1).GenerateCipherPassword (default);
		await _userRepository.ReceivedWithAnyArgs (1).TryGetOneAsync (filter: default, navigationProperties: default!);
		_tokenProvider.ReceivedWithAnyArgs (1).CreateToken (default!);
		await _refreshTokenRepository.ReceivedWithAnyArgs (1).TryGetFirstAsync (filter: default, orderBy: default);
		_ = _dateTimeProvider.Received ().UtcNow;
	}

	[Fact]
	public async Task LoginAsync_Should_Return_TokenInfoResponse_With_Newly_RefreshToken ()
	{
		// Given
		var userDb = new User
		{
			UserId = 1,
			Email = "test@test.com",
			Passwords = new List<Password>
			{
				new ()
				{
					Value = "TestCipher",
					PasswordId = 1,
					IsActive = true,
				}
			}
		};
		var refreshTokenDB = new RefreshToken
		{
			RefreshTokenId = 1,
			Value = "refreshToken",
			UserId = 1,
			ExpiredAt = new DateTime (2022, 6, 14)
		};
		var payload = new LoginPayload
		{
			Email = userDb.Email,
			Password = "password"
		};
		var tokenInfoResponse = new TokenInfoResponse
		{
			Token = "token",
			RefreshToken = "newRefreshToken"
		};
		_passwordProvider.GenerateCipherPassword (payload.Password).Returns ("TestCipher");
		_userRepository.TryGetOneAsync (filter: Arg.Any<Expression<Func<User, bool>>> (), navigationProperties: Arg.Any<Expression<Func<User, object>>[]> ()).Returns (userDb);
		_tokenProvider.CreateToken (Arg.Any<User> ()).Returns ("token");
		_refreshTokenRepository
			.TryGetFirstAsync (
				filter: Arg.Any<Expression<Func<RefreshToken, bool>>> (),
				orderBy: Arg.Any<Func<IQueryable<RefreshToken>, IOrderedQueryable<RefreshToken>>> ())
			.Returns (refreshTokenDB);
		_dateTimeProvider.UtcNow.Returns (new DateTime (2023, 6, 14));
		_tokenProvider.BuildRefreshToken (userDb, Arg.Any<DateTime> ()).Returns (new RefreshToken
		{
			RefreshTokenId = 2,
			ExpiredAt = new DateTime (2024, 6, 14),
			UserId = 1,
			Value = "newRefreshToken"
		});
		_unitOfWork.SaveChangesAsync ().Returns (1);

		// When
		var result = await _authService.LoginAsync (payload);

		// Then
		result.Should ().BeEquivalentTo (tokenInfoResponse);
		_passwordProvider.ReceivedWithAnyArgs (1).GenerateCipherPassword (default);
		await _userRepository.ReceivedWithAnyArgs (1).TryGetOneAsync (filter: default, navigationProperties: default!);
		_tokenProvider.ReceivedWithAnyArgs (1).CreateToken (default!);
		await _refreshTokenRepository.ReceivedWithAnyArgs (1).TryGetFirstAsync (filter: default, orderBy: default);
		_ = _dateTimeProvider.Received ().UtcNow;
		_tokenProvider.ReceivedWithAnyArgs (1).BuildRefreshToken (default!, default);
		_refreshTokenRepository.ReceivedWithAnyArgs (1).Insert (default!);
		await _unitOfWork.Received (1).SaveChangesAsync ();
	}

	[Fact]
	public async Task LoginAsync_Should_Throw_SkuldExceptionType_UserLoginFailed_If_No_User_With_Email ()
	{
		// Given
		var payload = new LoginPayload
		{
			Email = "test@test.com",
			Password = "password"
		};
		_passwordProvider.GenerateCipherPassword (payload.Password).Returns ("TestCipher");
		_userRepository.TryGetOneAsync (filter: Arg.Any<Expression<Func<User, bool>>> (), navigationProperties: Arg.Any<Expression<Func<User, object>>[]> ()).Returns (Task.FromResult<User?> (null!));

		// When
		var action = () => _authService.LoginAsync (payload);

		// Then
		await action.Should ().ThrowAsync<SkuldException> ()
			.Where (ex => ex.SkuldExceptionType == SkuldErrorType.UserLoginFailed && ex.HttpStatusCode == HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task LoginAsync_Should_Throw_SkuldExceptionType_UserLoginFailed_If_Bad_Password ()
	{
		// Given
		var userDb = new User
		{
			UserId = 1,
			Email = "test@test.com",
			Passwords = new List<Password>
			{
				new ()
				{
					Value = "TestCipher",
					PasswordId = 1,
					IsActive = true,
				}
			}
		};
		var payload = new LoginPayload
		{
			Email = "test@test.com",
			Password = "password"
		};
		_passwordProvider.GenerateCipherPassword (payload.Password).Returns ("OtherCipher");
		_userRepository.TryGetOneAsync (filter: Arg.Any<Expression<Func<User, bool>>> (), navigationProperties: Arg.Any<Expression<Func<User, object>>[]> ()).Returns (userDb);

		// When
		var action = () => _authService.LoginAsync (payload);

		// Then
		await action.Should ().ThrowAsync<SkuldException> ()
			.Where (ex => ex.SkuldExceptionType == SkuldErrorType.UserLoginFailed && ex.HttpStatusCode == HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task LoginAsync_Should_Throw_SkuldExceptionType_UserLoginFailed_If_Good_Password_But_No_Active ()
	{
		// Given
		var userDb = new User
		{
			UserId = 1,
			Email = "test@test.com",
			Passwords = new List<Password>
			{
				new ()
				{
					Value = "TestCipher",
					PasswordId = 1,
					IsActive = false,
				},
				new ()
				{
					Value = "TestCipherActive",
					PasswordId = 1,
					IsActive = true,
				}
			}
		};
		var payload = new LoginPayload
		{
			Email = "test@test.com",
			Password = "password"
		};
		_passwordProvider.GenerateCipherPassword (payload.Password).Returns ("TestCipher");
		_userRepository.TryGetOneAsync (filter: Arg.Any<Expression<Func<User, bool>>> (), navigationProperties: Arg.Any<Expression<Func<User, object>>[]> ()).Returns (userDb);

		// When
		var action = () => _authService.LoginAsync (payload);

		// Then
		await action.Should ().ThrowAsync<SkuldException> ()
			.Where (ex => ex.SkuldExceptionType == SkuldErrorType.UserLoginFailed && ex.HttpStatusCode == HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task LoginAsync_Should_Throw_SkuldExceptionType_UserLoginFailed_If_No_RefreshToken_Found ()
	{
		// Given
		var userDb = new User
		{
			UserId = 1,
			Email = "test@test.com",
			Passwords = new List<Password>
			{
				new ()
				{
					Value = "TestCipher",
					PasswordId = 1,
					IsActive = true,
				}
			}
		};
		var payload = new LoginPayload
		{
			Email = "test@test.com",
			Password = "password"
		};
		_passwordProvider.GenerateCipherPassword (payload.Password).Returns ("TestCipher");
		_userRepository.TryGetOneAsync (filter: Arg.Any<Expression<Func<User, bool>>> (), navigationProperties: Arg.Any<Expression<Func<User, object>>[]> ()).Returns (userDb);
		_tokenProvider.CreateToken (Arg.Any<User> ()).Returns ("token");
		_refreshTokenRepository
			.TryGetFirstAsync (
				filter: Arg.Any<Expression<Func<RefreshToken, bool>>> (),
				orderBy: Arg.Any<Func<IQueryable<RefreshToken>, IOrderedQueryable<RefreshToken>>> ())
			.Returns (Task.FromResult<RefreshToken?> (null));

		// When
		var action = () => _authService.LoginAsync (payload);

		// Then
		await action.Should ().ThrowAsync<SkuldException> ()
			.Where (ex => ex.SkuldExceptionType == SkuldErrorType.UserLoginFailed && ex.HttpStatusCode == HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task ValidRefreshToken_Should_Return_New_Token ()
	{
		// Given
		var userId = 1;
		var refreshTokenPayload = new RefreshTokenPayload { RefreshToken = "refreshToken" };
		_refreshTokenRepository.TryGetOneAsync (filter: Arg.Any<Expression<Func<RefreshToken, bool>>> ()).Returns (new RefreshToken
		{
			Value = "refreshToken",
			RefreshTokenId = 1,
			UserId = 1,
		});
		_userRepository.TryGetByIdAsync (Arg.Any<long> ()).Returns (new User { UserId = userId });
		_tokenProvider.CreateToken (Arg.Any<User> ()).Returns ("token");

		// When
		var result = await _authService.ValidRefreshTokenAsync (userId, refreshTokenPayload);

		// Then
		result.Should ().Be ("token");
		await _refreshTokenRepository.ReceivedWithAnyArgs (1).TryGetOneAsync (filter: default);
		await _userRepository.ReceivedWithAnyArgs (1).TryGetByIdAsync (default!);
		_tokenProvider.ReceivedWithAnyArgs (1).CreateToken (default!);
	}

	[Fact]
	public async Task ValidRefreshToken_Should_Throw_SkuldExceptionType_RefreshTokenInvalid ()
	{
		// Given
		var userId = 1;
		var refreshTokenPayload = new RefreshTokenPayload { RefreshToken = "refreshToken" };
		_refreshTokenRepository.TryGetOneAsync (filter: Arg.Any<Expression<Func<RefreshToken, bool>>> ()).Returns (Task.FromResult<RefreshToken?> (null));

		// When
		var action = () => _authService.ValidRefreshTokenAsync (userId, refreshTokenPayload);

		// Then
		await action.Should ().ThrowAsync<SkuldException> ()
			.Where (ex => ex.SkuldExceptionType == SkuldErrorType.RefreshTokenInvalid && ex.HttpStatusCode == HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task ValidRefreshToken_Should_Throw_SkuldExceptionType_UserNotFound ()
	{
		// Given
		var userId = 1;
		var refreshTokenPayload = new RefreshTokenPayload { RefreshToken = "refreshToken" };
		_refreshTokenRepository.TryGetOneAsync (filter: Arg.Any<Expression<Func<RefreshToken, bool>>> ()).Returns (new RefreshToken
		{
			Value = "refreshToken",
			RefreshTokenId = 1,
			UserId = 2,
		});
		_userRepository.TryGetByIdAsync (userId).Returns (Task.FromResult<User?> (null));
		// When
		var action = () => _authService.ValidRefreshTokenAsync (userId, refreshTokenPayload);

		// Then
		await action.Should ().ThrowAsync<SkuldException> ()
			.Where (ex => ex.SkuldExceptionType == SkuldErrorType.UserNotFound && ex.HttpStatusCode == HttpStatusCode.InternalServerError);
	}
}
