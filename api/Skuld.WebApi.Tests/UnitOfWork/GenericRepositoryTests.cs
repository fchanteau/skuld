using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NSubstitute;
using Skuld.Data;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using System.Linq.Expressions;

namespace Skuld.WebApi.Tests.UnitOfWork
{
	public class GenericRepositoryTests
	{
		private readonly SkuldContext _context;

		private readonly IGenericRepository<User> _userRepository;
		private static IEnumerable<User> _users;

		public GenericRepositoryTests ()
		{
			_context = Substitute.For<SkuldContext> ();

			//_context.When(x => x.Set<User>()).Do(_ => _userRepository.DbSet = (IQueryable<User>) new List<User> ());
			_users = FakeUsers ();

			var fakeDbSet = FakeDbSet (_users);

			_context.Set<User> ().Returns (fakeDbSet);
			_userRepository = new GenericRepository<User> (_context);
		}

		[Fact]
		public async Task GetCollectionAsync_Should_Return_All_Items ()
		{
			// Given / When
			var response = await _userRepository.GetCollectionAsync ();

			// Then
			response.Should ().NotBeEmpty ();
			response.Should ().HaveCount (2);
			response.Should ().BeEquivalentTo (_users);
		}

		[Fact]
		public async Task GetCollectionAsync_Should_Return_Filtered_Items ()
		{
			// Given / When
			var response = await _userRepository.GetCollectionAsync (filter: user => user.FirstName == "Test1");

			// Then
			response.Should ().NotBeEmpty ();
			response.Should ().HaveCount (1);
			response.First ().Should ().BeEquivalentTo (_users.First ());
		}

		[Fact]
		public async Task GetCollectionAsync_Should_Return_Ordered_Items ()
		{
			// Given / When
			var response = await _userRepository.GetCollectionAsync (orderBy: x => x.OrderByDescending (User => User.UserId));

			// Then
			response.Should ().NotBeEmpty ();
			response.Should ().HaveCount (2);
			response.Should ().BeEquivalentTo (_users.Reverse ());
		}

		[Fact]
		public async Task GetCollectionAsync_Should_Return_Items_With_Skip_Applied ()
		{
			// Given / When
			var response = await _userRepository.GetCollectionAsync (skip: 1);

			// Then
			response.Should ().NotBeEmpty ();
			response.Should ().HaveCount (1);
			response.First ().Should ().BeEquivalentTo (_users.ElementAt (1));
		}

		[Fact]
		public async Task GetCollectionAsync_Should_Return_Items_With_Take_Applied ()
		{
			// Given / When
			var response = await _userRepository.GetCollectionAsync (take: 1);

			// Then
			response.Should ().NotBeEmpty ();
			response.Should ().HaveCount (1);
			response.First ().Should ().BeEquivalentTo (_users.First ());
		}

		[Fact]
		public async Task GetCollectionAsync_Should_Return_Items_With_Select_Transformer ()
		{
			// Given
			var expected = new string[] { "test1@test.com", "test2@test.com" };

			// Given / When
			var response = await _userRepository.GetCollectionAsync (selector: x => x.Email);

			// Then
			response.Should ().NotBeEmpty ();
			response.Should ().HaveCount (2);
			response.Should ().BeEquivalentTo (expected);
		}

		// TODO FCU : Check to mock AsNoTracking
		//[Fact]
		//public async Task GetCollectionAsync_Should_Return_Items_Tracked ()
		//{
		//	// Given / When
		//	var response = await _userRepository.GetCollectionAsync (trackingEnabled: true);

		//	// Then
		//	response.Should ().NotBeEmpty ();
		//	response.Should ().HaveCount (2);
		//	response.Should ().BeEquivalentTo (_users);

		//	_context.Users.DidNotReceive ().AsNoTracking ();
		//}

		private DbSet<User> FakeDbSet (IEnumerable<User> data)
		{

			var _data = data.AsQueryable ();
			var fakeDbSet = Substitute.For<DbSet<User>, IQueryable<User>, IAsyncEnumerable<User>> ();
			var castMockSet = (IQueryable<User>)fakeDbSet;

			castMockSet.Provider.Returns (new TestAsyncQueryProvider<User> (_data.Provider));
			castMockSet.Expression.Returns (_data.Expression);
			castMockSet.ElementType.Returns (_data.ElementType);
			((IAsyncEnumerable<User>)fakeDbSet).GetAsyncEnumerator ().Returns (new TestAsyncEnumerator<User> (_data.GetEnumerator ()));

			return fakeDbSet;
		}

		private static IEnumerable<User> FakeUsers () =>
			new List<User> ()
			{
				new ()
				{
					Email = "test1@test.com",
					FirstName = "Test1",
					LastName = "Test1",
					Role = 1,
					UserId = 1,
					Passwords = new List<Password>
					{
						new ()
						{
							PasswordId = 1,
							IsActive = true,
							Value = "password",
							CreatedAt = new DateTime(2023, 6, 14)
						}
					}
				},
				new ()
				{
					Email = "test2@test.com",
					FirstName = "Test2",
					LastName = "Test2",
					Role = 2,
					UserId = 2,
					Passwords = new List<Password>
					{
						new ()
						{
							PasswordId = 2,
							IsActive = true,
							Value = "password",
							CreatedAt = new DateTime(2023, 6, 14)
						}
					}
				}
			};
	}

	internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
	{
		private readonly IQueryProvider _inner;

		internal TestAsyncQueryProvider (IQueryProvider inner)
		{
			_inner = inner;
		}

		public IQueryable CreateQuery (Expression expression)
		{
			return new TestAsyncEnumerable<TEntity> (expression);
		}

		public IQueryable<TElement> CreateQuery<TElement> (Expression expression)
		{
			return new TestAsyncEnumerable<TElement> (expression);
		}

		public TResult Execute<TResult> (Expression expression)
		{
			return _inner.Execute<TResult> (expression);
		}

		public IAsyncEnumerable<TResult> ExecuteAsync<TResult> (Expression expression)
		{
			return new TestAsyncEnumerable<TResult> (expression);
		}

		TResult IAsyncQueryProvider.ExecuteAsync<TResult> (Expression expression, CancellationToken cancellationToken)
		{
			return Execute<TResult> (expression);
		}

		public object? Execute (Expression expression)
		{
			return _inner.Execute (expression);
		}
	}

	internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
	{
		public TestAsyncEnumerable (IEnumerable<T> enumerable)
			: base (enumerable)
		{ }

		public TestAsyncEnumerable (Expression expression)
			: base (expression)
		{ }

		public IAsyncEnumerator<T> GetAsyncEnumerator (CancellationToken cancellationToken = default)
		{
			return new TestAsyncEnumerator<T> (this.AsEnumerable ().GetEnumerator ());
		}

		IQueryProvider IQueryable.Provider
		{
			get { return new TestAsyncQueryProvider<T> (this); }
		}
	}

	internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
	{
		private readonly IEnumerator<T> _inner;

		public TestAsyncEnumerator (IEnumerator<T> inner)
		{
			_inner = inner;
		}

		public void DisposeAsync ()
		{
			_inner.Dispose ();
		}

		public T Current
		{
			get
			{
				return _inner.Current;
			}
		}

		public Task<bool> MoveNextAsync (CancellationToken cancellationToken)
		{
			return Task.FromResult (_inner.MoveNext ());
		}

		public ValueTask<bool> MoveNextAsync ()
		{
			return ValueTask.FromResult (_inner.MoveNext ());
		}

		ValueTask IAsyncDisposable.DisposeAsync ()
		{
			return ValueTask.CompletedTask;
		}
	}
}
