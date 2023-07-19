using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Exceptions;
using Skuld.WebApi.Features.Auth;
using Skuld.WebApi.Features.Auth.Dto;
using Skuld.WebApi.Helpers;
using System.Linq.Expressions;
using System.Net;

namespace Skuld.WebApi.Tests.Features.Auth
{
	public class AuthServiceTests
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<User> _userRepository;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ITokenProvider _tokenProvider;

		private readonly AuthService _authService;

		public AuthServiceTests ()
		{
			_unitOfWork = Substitute.For<IUnitOfWork> ();
			_userRepository = Substitute.For<IGenericRepository<User>> ();
			_dateTimeProvider = Substitute.For<IDateTimeProvider> ();
			_tokenProvider = Substitute.For<ITokenProvider> ();

			_unitOfWork.UserRepository.Returns (_userRepository);

			_authService = new AuthService (_unitOfWork, _dateTimeProvider, _tokenProvider, NullLogger<AuthService>.Instance);
		}

		[Fact]
		public async Task GetUserAsync_Should_Return_UserResponse ()
		{
			// Given
			var dbUser = new User
			{
				Email = "test@test.com",
				FirstName = "Test",
				LastName = "Test",
				Role = 1,
				UserId = 1
			};
			var expectedResponse = new UserResponse
			{
				Email = "test@test.com",
				FirstName = "Test",
				LastName = "Test",
				Role = Role.User,
				UserId = 1
			};

			_userRepository.TryGetFirstAsync (filter: Arg.Any<Expression<Func<User, bool>>> ()).Returns (dbUser);

			// When
			var response = await _authService.GetUserAsync (1);

			// Then
			response.Should ().NotBeNull ();
			response.Should ().BeEquivalentTo (expectedResponse);
		}

		[Fact]
		public async Task GetUserAsync_Should_Throw_SkuldException_With_Type_UserNotFound_And_HttpStatusCode_NotFound ()
		{
			// Given
			_userRepository.TryGetFirstAsync (filter: Arg.Any<Expression<Func<User, bool>>> ()).Returns ((User)null!);

			// When
			var action = () => _authService.GetUserAsync (1);

			// Then
			await action.Should ().ThrowAsync<SkuldException> ()
				.Where (ex => ex.SkuldExceptionType == SkuldExceptionType.UserNotFound && ex.HttpStatusCode == HttpStatusCode.NotFound);
		}
	}
}
