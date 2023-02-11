using System;
using System.ComponentModel.DataAnnotations;

namespace EFR.NetworkObservability.RabbitMQ;

/// <summary>
/// Holds the connection details for connection to RabbitMQ.
/// Validation is performed through annotations.
/// </summary>
public class ConnectionDetails
{
	/// <summary>
	/// Connection string used to establish connection
	/// </summary>
	[Required]
	public string? ConnectionString
	{
		get; set;
	}

	/// <summary>
	/// User Name used  to establish connection
	/// </summary>
	[Required]
	public string? UserName
	{
		get; set;
	}

	/// <summary>
	/// Password used to establish connection
	/// </summary>
	[Required]
	public string? Password
	{
		get; set;
	}

	/// <summary>
	/// Constructs the URI used to connect to rabbitmq.
	/// Only has a getter, since value is constructed from other properties.
	/// </summary>
	public Uri Uri
	{
		get => new($"amqp://{UserName}:{Password}@{ConnectionString}/");
	}
}
