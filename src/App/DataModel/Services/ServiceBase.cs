using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

namespace EFR.NetworkObservability.DataModel.Services;

/// <summary>
/// Abstract Class for interacting with EF dbcontext
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class ServiceBase
{
	private readonly DbContext context;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="context">Db context object</param>
	public ServiceBase(DbContext context)
	{
		ArgumentNullException.ThrowIfNull(context);

		this.context = context;

		this.context.Database.EnsureCreated();
	}

	/// <summary>
	/// Asynchronously inserts one entity into a db.
	/// </summary>
	/// <typeparam name="T">Generic type of dbset</typeparam>
	/// <param name="entity">Entity to create</param>
	/// <returns>Awaitable Task</returns>
	protected async Task CreateAsync<T>(T entity) where T : class
		=> await context.Set<T>().AddAsync(entity);

	/// <summary>
	/// Inserts one entity into db.
	/// </summary>
	/// <typeparam name="T">Generic type of dbset</typeparam>
	/// <param name="entity">Entity to create</param>
	protected void Create<T>(T entity) where T : class
		=> context.Set<T>().Add(entity);

	/// <summary>
	/// Asynchronously inserts a list of entities into a db.
	/// </summary>
	/// <typeparam name="T">Generic type of dbset</typeparam>
	/// <param name="entity">Entity to create</param>
	/// <returns>Awaitable Task</returns>
	protected async Task CreateEntitiesAsync<T>(IEnumerable<T> entity) where T : class
		=> await context.Set<T>().AddRangeAsync(entity);

	/// <summary>
	/// Inserts a list of entities into a db.
	/// </summary>
	/// <typeparam name="T">Generic type of dbset</typeparam>
	/// <param name="entity">Entity to create</param>
	protected void CreateEntities<T>(IEnumerable<T> entity) where T : class
		=> context.Set<T>().AddRange(entity);

	/// <summary>
	/// Asynchronously Bulk inserts entities into a db.
	/// </summary>
	/// <typeparam name="T">Generic type of dbset</typeparam>
	/// <param name="entities">List of entities to insert</param>
	/// <returns>Awaitable Task</returns>
	protected async Task BulkCreateAsync<T>(IList<T> entities) where T : class
	{
		try
		{
			await context.BulkInsertAsync(entities);
		}
		catch (InvalidOperationException)
		{
			await CreateEntitiesAsync(entities);
		}

	}

	/// <summary>
	/// Bulk inserts entities into a db.
	/// </summary>
	/// <typeparam name="T">Generic type of dbset</typeparam>
	/// <param name="entities">List of entities to insert</param>
	protected void BulkCreate<T>(IList<T> entities) where T : class
	{
		try
		{
			context.BulkInsert(entities);
		}
		catch (InvalidOperationException)
		{
			CreateEntities(entities);
		}

	}

	/// <summary>
	/// Asynchronously retrieves all entities from db.
	/// </summary>
	/// <typeparam name="T">Generic type of dbset</typeparam>
	/// <returns>Awaitable Task of IEnumerable</returns>
	protected async Task<IEnumerable<T>> RetrieveAllAsync<T>() where T : class
		=> await context.Set<T>().ToListAsync();

	/// <summary>
	/// Retrieves all entities from db.
	/// </summary>
	/// <typeparam name="T">Generic type of dbset</typeparam>
	protected IEnumerable<T> RetrieveAll<T>() where T : class
		=> context.Set<T>().AsEnumerable();

	/// <summary>
	/// Retrieves all entities as queryable
	/// </summary>
	/// <typeparam name="T">Generic type of dbset</typeparam>
	/// <returns>Awaitable Task of IQueryable</returns>
	protected IQueryable<T> QueryAll<T>() where T : class
		=> context.Set<T>().AsQueryable();

	/// <summary>
	/// Asynchronously Save Changes made to DB.
	/// </summary>
	/// <returns>Awaitable Task</returns>
	protected async Task SaveAsync()
		=> await context.SaveChangesAsync();

	/// <summary>
	/// Save Changes made to DB.
	/// </summary>
	protected void Save()
		=> context.SaveChanges();
}
