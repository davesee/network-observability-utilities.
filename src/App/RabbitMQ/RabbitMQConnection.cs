using System;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EFR.NetworkObservability.RabbitMQ;

/// <summary>
/// Responsible of managing Connections to RabbitMQ.
/// </summary>
public class RabbitMQConnection : IDisposable
{
	// RabbitMQ connection details
	private readonly ConnectionDetails connectionDetails;

	// RabbitMQ connection factory
	private readonly IConnectionFactory connectionFactory;

	// RabbitMQ connection
	private IConnection? connection;

	// RabbitMQ channel connection
	private IModel? channel;

	private bool disposedValue;


	/// <summary>
	/// Constructs an instance with fields and sets their values.
	/// </summary>
	/// <param name="connectionDetails">Details required to connect to RabbitMQ.</param>
	/// <param name="connectionFactory">[Optional] Factory responsible for creating RabbitMQ connections.</param>
	/// <exception cref="ArgumentNullException">Throws Exception if connection details aren't provided.</exception>
	public RabbitMQConnection(
		IOptions<ConnectionDetails> connectionDetails,
		IConnectionFactory? connectionFactory = null)
	{
		try
		{
			this.connectionDetails = connectionDetails.Value;
		}
		catch (NullReferenceException)
		{
			throw new ArgumentNullException(nameof(connectionDetails));
		}

		this.connectionFactory = connectionFactory ??
			new ConnectionFactory();
	}

	/// <summary>
	/// Creates a connection to RabbitMQ.
	/// Will reuse connection if one is already open.
	/// </summary>
	/// <returns>RabbitMQ connection.</returns>
	public IConnection GetConnection()
	{
		if (connection is null || !connection.IsOpen)
		{
			connectionFactory.Uri = connectionDetails.Uri;
			connection = connectionFactory.CreateConnection();
		}

		return connection;
	}

	/// <summary>
	/// Creates a RabbitMQ channel.
	/// Will reuse channel connection if one is already open.
	/// </summary>
	/// <returns>RabbitMQ channel</returns>
	public IModel GetChannel()
	{
		if (channel is null || !channel.IsOpen)
		{
			channel = GetConnection().CreateModel();
		}

		return channel;
	}

	/// <summary>
	/// Closes RabbitMQ connection and channel if they are open.
	/// </summary>
	public void Close()
	{
		channel?.Close();
		connection?.Close();
	}

	/// <summary>
	/// Dispose of unmanaged resources and Suppress finalization.
	/// </summary>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// free unmanaged resources (unmanaged objects) and override a finalizer
	/// </summary>
	/// <param name="disposing">Is object being disposed</param>
	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				Close();
				channel?.Dispose();
				connection?.Dispose();
			}
			disposedValue = true;
		}
	}

	/// <summary>
	/// Deconstructor
	/// </summary>
	~RabbitMQConnection()
	{
		Dispose(disposing: false);
	}
}
