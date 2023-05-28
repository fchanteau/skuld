using Microsoft.EntityFrameworkCore;
using Skuld.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Skuld.Data.UnitOfWork
{
	public interface IGenericRepository<TEntity> where TEntity : class, IEntity
	{
		SkuldContext Context { get; set; }
		DbSet<TEntity> DbSet { get; set; }

		void Insert (TEntity entity);
		void Update (TEntity entity);
		void Delete (TEntity entity);

		Task<IEnumerable<TEntity>> GetCollectionAsync (Expression<Func<TEntity, bool>>? filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
			int? skip = null,
			int? take = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties);

		Task<IEnumerable<TResult>> GetCollectionAsync<TResult> (Expression<Func<TEntity, TResult>> selector,
			Expression<Func<TEntity, bool>>? filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
			int? skip = null,
			int? take = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties) where TResult : class;

		Task<TEntity?> TryGetOneAsync (
			Expression<Func<TEntity, bool>>? filter = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties);

		Task<TResult?> TryGetOneAsync<TResult> (
			Expression<Func<TEntity, TResult>> selector,
			Expression<Func<TEntity, bool>>? filter = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties) where TResult : class;

		Task<TEntity?> TryGetFirstAsync (
			Expression<Func<TEntity, bool>>? filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties);

		Task<TResult?> TryGetFirstAsync<TResult> (
			Expression<Func<TEntity, TResult>> selector,
			Expression<Func<TEntity, bool>>? filter = null,
			bool trackingEnabled = false,
			params Expression<Func<TEntity, object>>[] navigationProperties) where TResult : class;

		Task<TEntity?> TryGetByIdAsync (object id);

		Task<int> CountAsync (Expression<Func<TEntity, bool>>? filter = null);

		Task<bool> AnyAsync (Expression<Func<TEntity, bool>>? filter = null);

		IQueryable<TEntity> AsQueryable ();
	}
}
