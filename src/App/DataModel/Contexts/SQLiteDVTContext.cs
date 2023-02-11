using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace EFR.NetworkObservability.DataModel.Contexts;
/// <summary>
/// Context for interacting with SQLite
/// </summary>
[ExcludeFromCodeCoverage]
public class SQLitePcapContext : DbContext
{
	private readonly string connectionString;

	/// <summary>
	/// Pcap Index sqlite dbset
	/// </summary>
	public virtual DbSet<SQLitePcapIndex> PcapIndices => Set<SQLitePcapIndex>();

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="connectionString">ConnectionString for sqlite</param>
	public SQLitePcapContext(string connectionString)
	{
		this.connectionString = connectionString;
	}

	/// <summary>
	/// Configures the contexts options
	/// </summary>
	/// <param name="optionsBuilder">context options builder</param>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite(connectionString);
	}

	/// <summary>
	/// Configure the data model
	/// </summary>
	/// <param name="modelBuilder">Used to define the model</param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<SQLitePcapIndex>().HasNoKey();
	}
}
