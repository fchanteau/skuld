using Microsoft.EntityFrameworkCore;
using Skuld.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Skuld.Data.UnitOfWork
{
	public class GenericRepository<TEntity> where TEntity : class, IEntity
	{
		#region Properties

		protected SkuldContext _context;
		protected DbSet<TEntity> _dbSet;

		#endregion

		#region Constructor

		public GenericRepository (SkuldContext context)
		{
			_context = context;
			_dbSet = _context.Set<TEntity> ();
		}

		#endregion

		#region ADD / UPDATE / DELETE

		public void Insert (TEntity entity)
		{
			_dbSet.Add (entity);
		}

		public void Update (TEntity entity)
		{
			_context.Entry (entity).State = EntityState.Modified;
		}

		public void Delete (TEntity entity)
		{
			_dbSet.Remove (entity);
		}

		#endregion

		#region GET

		public async Task<IEnumerable<TEntity>> GetCollectionAsync (Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			int? skip = null,
			int? take = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties)
		{
			IQueryable<TEntity> query = _dbSet;

			if (filter != null)
			{
				query = query.Where (filter);
			}

			foreach (var navigationProperty in navigationProperties)
			{
				query = query.Include (navigationProperty);
			}

			if (orderBy != null)
			{
				query = orderBy (query);
			}

			if (skip.HasValue)
			{
				query = query.Skip (skip.Value);
			}

			if (take.HasValue)
			{
				query = query.Take (take.Value);
			}

			if (!trackingEnabled)
			{
				query = query.AsNoTracking ();
			}

			return await query.ToListAsync ();
		}

		public async Task<IEnumerable<TResult>> GetCollectionAsync<TResult> (Expression<Func<TEntity, TResult>> selector,
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			int? skip = null,
			int? take = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties) where TResult : class
		{
			IQueryable<TEntity> query = _dbSet;

			if (filter != null)
			{
				query = query.Where (filter);
			}

			foreach (var navigationProperty in navigationProperties)
			{
				query = query.Include (navigationProperty);
			}

			if (orderBy != null)
			{
				query = orderBy (query);
			}

			if (skip.HasValue)
			{
				query = query.Skip (skip.Value);
			}

			if (take.HasValue)
			{
				query = query.Take (take.Value);
			}

			if (!trackingEnabled)
			{
				query = query.AsNoTracking ();
			}

			return await query.Select (selector).ToListAsync ();
		}

		public virtual Task<TEntity> TryGetOneAsync (
			Expression<Func<TEntity, bool>> filter = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties)
		{
			IQueryable<TEntity> query = _dbSet;

			if (filter != null)
			{
				query = query.Where (filter);
			}

			foreach (var navigationProperty in navigationProperties)
			{
				query = query.Include (navigationProperty);
			}

			if (!trackingEnabled)
			{
				query = query.AsNoTracking ();
			}

			return query.SingleOrDefaultAsync ();
		}

		public virtual Task<TResult> TryGetOneAsync<TResult> (
			Expression<Func<TEntity, TResult>> selector,
			Expression<Func<TEntity, bool>> filter = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties) where TResult : class
		{
			IQueryable<TEntity> query = _dbSet;

			if (filter != null)
			{
				query = query.Where (filter);
			}

			foreach (var navigationProperty in navigationProperties)
			{
				query = query.Include (navigationProperty);
			}

			if (!trackingEnabled)
			{
				query = query.AsNoTracking ();
			}

			return query.Select (selector).SingleOrDefaultAsync ();
		}

		public virtual Task<TEntity> TryGetFirstAsync (
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties)
		{
			IQueryable<TEntity> query = _dbSet;

			if (filter != null)
			{
				query = query.Where (filter);
			}

			foreach (var navigationProperty in navigationProperties)
			{
				query = query.Include (navigationProperty);
			}

			if (orderBy != null)
			{
				query = orderBy (query);
			}

			if (!trackingEnabled)
			{
				query = query.AsNoTracking ();
			}

			return query.FirstOrDefaultAsync ();
		}

		public virtual Task<TResult> TryGetFirstAsync<TResult> (
			Expression<Func<TEntity, TResult>> selector,
			Expression<Func<TEntity, bool>> filter = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties) where TResult : class
		{
			IQueryable<TEntity> query = _dbSet;

			if (filter != null)
			{
				query = query.Where (filter);
			}

			foreach (var navigationProperty in navigationProperties)
			{
				query = query.Include (navigationProperty);
			}

			if (!trackingEnabled)
			{
				query = query.AsNoTracking ();
			}

			return query.Select (selector).FirstOrDefaultAsync ();
		}

		public virtual Task<TEntity> TryGetByIdAsync (object id)
		{
			return _dbSet.FindAsync (id).AsTask ();
		}

		public virtual Task<int> CountAsync (Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = _dbSet;

			if (filter != null)
			{
				query = query.Where (filter);
			}

			return query.CountAsync ();
		}

		public virtual Task<bool> AnyAsync (Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = _dbSet;

			if (filter != null)
			{
				query = query.Where (filter);
			}

			return query.AnyAsync ();
		}

		public IQueryable<TEntity> AsQueryable ()
		{
			return this._dbSet.AsQueryable ();
		}

		#endregion
	}
}
