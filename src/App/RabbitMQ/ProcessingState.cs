namespace EFR.NetworkObservability.RabbitMQ;

/// <summary>
/// Processing states for an individual file flowing through the solution
/// </summary>
public enum ProcessingState
{
	/// <summary>
	/// The Pcap has been ingested
	/// </summary>
	Ingested,

	/// <summary>
	/// The Pcap has been validated
	/// </summary>
	Validated,

	/// <summary>
	/// The Pcap has been ingested and is being processed by PcapProcessor
	/// </summary>
	Processing,

	/// <summary>
	/// The Pcap has been ingested and unzipped by PcapProcessor and its PCAP file has been processed
	/// </summary>
	PcapProcessed,

	/// <summary>
	/// The Pcap has been processed and inserted into the DB PcapProcessor and event based coorelation has been performed
	/// </summary>
	EventProcessed,

	/// <summary>
	/// Event based coorelation has been performed and Network Observability data has been created
	/// </summary>
	NOStatsCreated
}
