namespace EFR.NetworkObservability.DataModel;

/// <summary>
/// What is the current status of the job?
/// </summary>
public enum JobStatus
{
	/// <summary>
	/// Processing of the Pcap File has been successfully completed.
	/// </summary>
	Completed,
	/// <summary>
	/// Processing of the Pcap File has started.
	/// </summary>
	InProgress,
	/// <summary>
	/// Processing of the Pcap file failed.
	/// </summary>
	Failed,
	/// <summary>
	/// The Pcap file failed validation checks.
	/// </summary>
	ValidationFailed
}
