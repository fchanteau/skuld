using Skuld.Data.Entities;
using System;
using System.Threading.Tasks;

namespace Skuld.Data.UnitOfWork
{
	public interface IUnitOfWork
	{
		IGenericRepository<User> UserRepository { get; }
		IGenericRepository<RefreshToken> RefreshTokenRepository { get; }
		IGenericRepository<Password> PasswordRepository { get; }
		Task<int> SaveChangesAsync ();

	}

	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		#region Private Properties

		private readonly SkuldContext _context;
		private bool _disposed;

		private IGenericRepository<User>? _userRepository;
		private IGenericRepository<RefreshToken>? _refreshTokenRepository;
		private IGenericRepository<Password>? _passwordRepository;

		#endregion;

		#region Constructor

		public UnitOfWork (SkuldContext context)
		{
			_context = context;
		}

		#endregion

		#region Accessors

		public IGenericRepository<User> UserRepository =>
			_userRepository ??= new GenericRepository<User> (_context);


		public IGenericRepository<RefreshToken> RefreshTokenRepository
			=> _refreshTokenRepository ??= new GenericRepository<RefreshToken> (_context);

		public IGenericRepository<Password> PasswordRepository
			=> _passwordRepository ??= new GenericRepository<Password> (_context);


		#endregion

		#region Dispose

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public void Dispose (bool disposing)
		{
			if (_disposed)
			{
				if (disposing)
				{
					_context.Dispose ();
				}
			}
			_disposed = true;
		}

		#endregion

		#region Public methods

		public Task<int> SaveChangesAsync ()
		{
			return _context.SaveChangesAsync ();
		}

		#endregion
	}
}
