using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EFR.NetworkObservability.DataModel;

/// <summary>
/// Model for a EventmetaData
/// </summary>
[Table("EventMetaData")]
[ExcludeFromCodeCoverage]
public class EventMetaData
{
	/// <summary>
	/// Identity for the event data
	/// </summary>
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("eventMetaDataID")]
	public long EventMetaDataID
	{
		get;
		set;
	}

	/// <summary>
	/// Track the days with JulianDate
	/// </summary>
	[Column("julianDay")]
	public int JulianDay
	{
		get;
		set;
	}

	/// <summary>
	/// Ready status
	/// </summary>
	[Column("ready")]
	public bool Ready
	{
		get;
		set;
	}

	/// <summary>
	/// Reprocess status
	/// </summary>
	[Column("reprocess")]
	public bool Reprocess
	{
		get;
		set;
	}

	/// <summary>
	/// Track the intervals in seconds
	/// </summary>
	[Column("intervalInSeconds")]
	public int IntervalInSeconds
	{
		get;
		set;
	}
}
