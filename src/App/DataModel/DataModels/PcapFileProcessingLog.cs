using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace EFR.NetworkObservability.DataModel;

/// <summary>
/// Model for a job
/// </summary>
[Table("FileProcessingLog")]
[Index(nameof(ProcessingJobID), IsUnique = true)]
[ExcludeFromCodeCoverage]
public class PcapFileProcessingLog
{
	/// <summary>
	/// Identity for the job that can be a reference key for other tables
	/// </summary>
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("pcapFileProcessingLogID")]
	public long PcapFileProcessingLogID
	{
		get;
		set;
	}

	/// <summary>
	/// Original unique job processing ID
	/// </summary>
	[Column("processingJobID")]
	public Guid ProcessingJobID
	{
		get;
		set;
	}

	/// <summary>
	/// Original file name
	/// </summary>
	[Column("originalFileName")]
	[MaxLength(255)]
	public string? OriginalFileName
	{
		get;
		set;
	}

	/// <summary>
	/// Time the file was detected in the watch folder
	/// </summary>
	[Column("timestampDetected")]
	public DateTime TimestampDetected
	{
		get;
		set;
	}

	/// <summary>
	/// Size of the file
	/// </summary>
	[Column("fileSize")]
	public long FileSize
	{
		get;
		set;
	}

	/// <summary>
	/// Type of File
	/// </summary>
	[Column("fileType")]
	[MaxLength(10)]
	public string? FileType
	{
		get;
		set;
	}

	/// <summary>
	/// When the job started
	/// </summary>
	[Column("startDateTime")]
	public DateTime StartDateTime
	{
		get;
		set;
	}

	/// <summary>
	/// When the job ended
	/// </summary>
	[Column("endDateTime")]
	public DateTime? EndDateTime
	{
		get;
		set;
	}

	/// <summary>
	/// Current job status
	/// </summary>
	[Column("processingJobStatus")]
	public JobStatus ProcessingJobStatus
	{
		get;
		set;
	}

	/// <summary>
	/// Notes relevant to the job
	/// </summary>
	[Column("notes")]
	[MaxLength(int.MaxValue)]
	public string? Notes
	{
		get;
		set;
	}
}
