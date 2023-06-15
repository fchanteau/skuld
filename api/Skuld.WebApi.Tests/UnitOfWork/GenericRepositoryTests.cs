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

		public GenericRepositoryTests ()
		{
			_context = Substitute.For<SkuldContext> ();

			//_context.When(x => x.Set<User>()).Do(_ => _userRepository.DbSet = (IQueryable<User>) new List<User> ());
			var items = new List<User> ()
			{
				new ()
				{
					Email = "test@test.com",
					FirstName = "Test",
					LastName = "Test",
					Role = 1,
					UserId = 1,
				}
			};

			var fakeDbSet = FakeDbSet (items);

			_context.Set<User> ().Returns (fakeDbSet);
			_userRepository = new GenericRepository<User> (_context);
		}

		[Fact]
		public async Task GetCollectionAsync_Should_Return_All_Items ()
		{
			// Given


			// When
			var response = await _userRepository.GetCollectionAsync ();

			// Then
			response.Should ().NotBeEmpty ();
			response.Should ().HaveCount (1);
			response.First ().Should ().NotBeNull ();
		}

		private DbSet<User> FakeDbSet (IEnumerable<User> data)
		{

			var _data = data.AsQueryable ();
			var fakeDbSet = Substitute.For<DbSet<User>, IQueryable<User>, IAsyncEnumerable<User>> ();
			var castMockSet = (IQueryable<User>)fakeDbSet;

			castMockSet.Provider.Returns (new TestAsyncQueryProvider<User> (_data.Provider));
			castMockSet.Expression.Returns (_data.Expression);
			castMockSet.ElementType.Returns (_data.ElementType);
			((IAsyncEnumerable<User>)fakeDbSet).GetAsyncEnumerator ().Returns (new TestAsyncEnumerator<User> (_data.GetEnumerator ()));
			//castMockSet.AsNoTracking ().Returns (castMockSet);

			return fakeDbSet;

		}
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
