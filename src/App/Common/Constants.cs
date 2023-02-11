namespace EFR.NetworkObservability.Common;

/// <summary>
/// Holds solution constants
/// </summary>
public static class Constants
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	/////////////////////////////////////////////////////////////////////////////////////////////////
	//               General
	/////////////////////////////////////////////////////////////////////////////////////////////////

	public const string JOB_ID_LOG_PREFIX = "JOB_PROC_ID:";

	/////////////////////////////////////////////////////////////////////////////////////////////////
	//               Environment Variables
	/////////////////////////////////////////////////////////////////////////////////////////////////

	// Directories
	public const string ERROR_DIRECTORY = "ERROR_DIRECTORY";
	public const string VALIDATION_DIRECTORY = "VALIDATION_DIRECTORY";
	public const string VALIDATED_DIRECTORY = "VALIDATED_DIRECTORY";
	public const string CONFIGURATION_DIRECTORY = "CONFIGURATION_DIRECTORY";
	public const string CONVERSION_DIRECTORY = "CONVERSION_DIRECTORY";
	public const string EVENT_META_DATA_DIRECTORY = "EVENT_META_DATA_DIRECTORY";
	public const string PCAP_EXTRACTION_DIRECTORY = "PCAP_EXTRACTION_DIRECTORY";

	// RabbitMQ
	public const string RABBITMQ_HOSTNAME = "RABBITMQ_HOSTNAME";
	public const string RABBITMQ_PORT = "RABBITMQ_PORT";
	public const string RABBITMQ_USERNAME = "RABBITMQ_USERNAME";

	//  pragma: allowlist nextline secret
	public const string RABBITMQ_PASSWORD = "RABBITMQ_PASSWORD";

	// RabbitMQ Queues
	public const string RABBITMQ_QUEUE = "queue";
	public const string PCAP_PROCESS_QUEUE = "PCAP_PROCESS_QUEUE";
	public const string EVENTDATA_PROCESS_QUEUE = "EVENTDATA_PROCESS_QUEUE";

	// Database
	public const string DB_CONNECTION_STRING = "DB_CONNECTION_STRING";

	// Polling fileWatcher
	public const string FILE_POLLING_INTERVAL = "FILE_POLLING_INTERVAL";

	public const string DATA_CHUNK_SIZE = "DATA_CHUNK_SIZE";

	// Elasticsearch
	public const string ELASTICSEARCH_URI = "ELASTICSEARCH_URI";
}
