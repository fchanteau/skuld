using Skuld.Data.Entities;
using System;

namespace Skuld.Data.UnitOfWork
{
	public class UnitOfWork : IDisposable
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

		public GenericRepository<User> UserRepository
		{
			get
			{
				if (_userRepository is null)
				{
					_userRepository = new GenericRepository<User> (_context);
				}
				return _userRepository;
			}
		}

		public GenericRepository<RefreshToken> RefreshTokenRepository
		{
			get
			{
				if (_refreshTokenRepository is null)
				{
					_refreshTokenRepository = new GenericRepository<RefreshToken> (_context);
				}
				return _refreshTokenRepository;
			}
		}

		public GenericRepository<Password> PasswordRepository
		{
			get
			{
				if (_passwordRepository is null)
				{
					_passwordRepository = new GenericRepository<Password> (_context);
				}
				return _passwordRepository;
			}
		}


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

		public int Save ()
		{
			return _context.SaveChanges ();
		}

		#endregion
	}
}
