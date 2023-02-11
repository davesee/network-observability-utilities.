using EFR.NetworkObservability.DataModel.Configurations;
using Microsoft.EntityFrameworkCore;
using DbConfigurationType = System.Data.Entity.DbConfigurationTypeAttribute;

namespace EFR.NetworkObservability.DataModel.Contexts;

/// <summary>
/// Database context information
/// </summary>
[DbConfigurationType(typeof(SQLServerConfiguration))]
public class PcapContext : DbContext
{
	/// <summary>
	/// Set of job logs
	/// </summary>
	public virtual DbSet<PcapFileProcessingLog> PcapFileProcessingLogs => Set<PcapFileProcessingLog>();

	/// <summary>
	/// Set of job logs
	/// </summary>
	public virtual DbSet<PcapIndex> PcapIndices => Set<PcapIndex>();

	/// <summary>
	/// Set of Pcap Meta Datas
	/// </summary>
	public virtual DbSet<PcapMetaData> PcapMetaDatas => Set<PcapMetaData>();

	/// <summary>
	/// Set of event data
	/// </summary>
	public virtual DbSet<EventMetaData> EventMetaDatas => Set<EventMetaData>();

	/// <summary>
	/// Default constructor
	/// </summary>
	public PcapContext(DbContextOptions<PcapContext> options) : base(options)
	{
	}

	/// <summary>
	/// Overrides the DbContext OnModelCreating method so we can set included columns in the IX_Pcapindices_IPs index.
	/// </summary>
	/// <param name="modelBuilder">ModelBuilder object.</param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		try
		{
			SqlServerIndexBuilderExtensions.IncludeProperties(modelBuilder.Entity<PcapIndex>()
																			.HasIndex(p => new { p.SourceIP, p.DestinationIP }, "IX_Pcapindices_IPs"),
																p => new { p.PcapIndexID, p.PcapFileProcessingLogID, p.Timestamp, p.JulianDay });
		}
		catch (System.Exception ex)
		{
			System.Console.WriteLine(ex.ToString());

			throw;
		}
	}
}
