using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EFR.NetworkObservability.DataModel;

/// <summary>
/// Model for a PcapMetaData
/// </summary>
[Table("PcapMetaData")]
[ExcludeFromCodeCoverage]
public class PcapMetaData
{
	/// <summary>
	/// Identity for the event data
	/// </summary>
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("pcapMetaDataID")]
	public long PcapMetaDataID
	{
		get;
		set;
	}

	/// <summary>
	/// Foreign key for the PcapFileProcessing Log table
	/// </summary>
	[ForeignKey("pcapFileProcessingLog")]
	[Column("pcapFileProcessingLogID")]
	public long? PcapFileProcessingLogID
	{
		get;
		set;
	}

	/// <summary>
	/// Foreign key for the PcapFileProcessing Log table
	/// </summary>
	public virtual PcapFileProcessingLog? PcapFileProcessingLog
	{
		get;
		set;
	}

	/// <summary>
	/// Track the days with JulianDate
	/// </summary>
	[Column("collectorID")]
	public Guid CollectorID
	{
		get;
		set;
	}

	/// <summary>
	/// Collector Name
	/// </summary>
	[MaxLength(30)]
	[Column("collectorName")]
	public string? CollectorName
	{
		get;
		set;
	}
}
