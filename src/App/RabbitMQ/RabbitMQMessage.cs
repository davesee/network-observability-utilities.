namespace EFR.NetworkObservability.RabbitMQ;

/// <summary>
/// A RabbitMQ message containing Job and Processing info
/// </summary>
public class RabbitMQMessage
{
	/// <summary>
	/// The Primary Key of the job tracking this file's progress
	/// </summary>
	public long JobPK
	{
		get; set;
	}

	/// <summary>
	/// The ID of the job tracking this file's progress
	/// </summary>
	public string? JobID
	{
		get; set;
	}

	/// <summary>
	/// The current processing state
	/// </summary>
	public ProcessingState State
	{
		get; set;
	}
}

