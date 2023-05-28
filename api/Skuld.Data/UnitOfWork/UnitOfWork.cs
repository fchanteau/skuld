using Skuld.Data.Entities;
using System;
using System.Threading.Tasks;

namespace Skuld.Data.UnitOfWork
{
	public interface IUnitOfWork
	{
		GenericRepository<User> UserRepository { get; }
		GenericRepository<RefreshToken> RefreshTokenRepository { get; }
		GenericRepository<Password> PasswordRepository { get; }
		Task<int> SaveChangesAsync ();

	}

	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		#region Private Properties

		private readonly SkuldContext _context;
		private bool _disposed;

		private GenericRepository<User> _userRepository;
		private GenericRepository<RefreshToken> _refreshTokenRepository;
		private GenericRepository<Password> _passwordRepository;

		#endregion;

		#region Constructor

		public UnitOfWork (SkuldContext context)
		{
			_context = context;
		}

		#endregion

		#region Accessors

		public GenericRepository<User> UserRepository =>
			_userRepository ??= new GenericRepository<User> (_context);


		public GenericRepository<RefreshToken> RefreshTokenRepository
			=> _refreshTokenRepository ??= new GenericRepository<RefreshToken> (_context);

		public GenericRepository<Password> PasswordRepository
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
