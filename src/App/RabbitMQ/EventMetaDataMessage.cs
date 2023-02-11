namespace EFR.NetworkObservability.RabbitMQ;

/// <summary>
/// A RabbitMQ message indication for a event meta data
/// </summary>
public class EventMetaDataMessage
{
	/// <summary>
	/// The Julian Date of the message
	/// </summary>
	public string? JulianDay
	{
		get; set;
	}

	/// <summary>
	/// The ready status of event meta data
	/// </summary>
	public string? Ready
	{
		get; set;
	}

	/// <summary>
	/// The processing state indicator
	/// </summary>
	public string? ReProcess
	{
		get; set;
	}

	/// <summary>
	/// The intervals in seconds indicator
	/// </summary>
	public string? IntervalInSeconds
	{
		get; set;
	}
}
