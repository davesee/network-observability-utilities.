namespace EFR.NetworkObservability.RabbitMQ;

/// <summary>
/// A RabbitMQ message indicating a Pcap was successfully validated
/// </summary>
public class PcapValidatedMessage : RabbitMQMessage
{
	/// <summary>
	/// The name of the validated Pcap file
	/// </summary>
	public string? PathAndFileName
	{
		get; set;
	}
}

