using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EFR.NetworkObservability.RabbitMQ;
/// <summary>
/// Provides methods for interacting with RabbitMQ
/// </summary>
public class RabbitMQMessenger
{
	private readonly RabbitMQConnection connection;

	/// <summary>
	/// Constructs an instance with fields and sets their values.
	/// </summary>
	/// <param name="connectionDetails">Details required to connect to RabbitMQ.</param>
	public RabbitMQMessenger(
		IOptions<ConnectionDetails> connectionDetails)
		: this(new RabbitMQConnection(connectionDetails))
	{
	}

	/// <summary>
	/// Constructs an instance with fields and sets their values.
	/// </summary>
	/// <param name="connection">Instance of RabbitMQ connection.</param>
	/// <exception cref="ArgumentNullException">Throws exception if RabbitMQ connection is null</exception>
	public RabbitMQMessenger(RabbitMQConnection connection)
	{
		this.connection = connection ??
			throw new ArgumentNullException(nameof(connection));
	}

	/// <summary>
	/// Pushes a message to RabbitMQ.
	/// </summary>
	/// <typeparam name="T">Generic type of message.</typeparam>
	/// <param name="message">Message to push.</param>
	/// <param name="queueName">Queue where message is pushed.</param>
	public void PushMessage<T>(T message, string queueName) where T : class
	{
		byte[] body = ConvertToBytes(message);

		IModel channel = connection.GetChannel();

		DeclareQueue(channel, queueName);

		IBasicProperties properties = channel.CreateBasicProperties();
		properties.Persistent = true;

		channel.BasicPublish(exchange: string.Empty,
			routingKey: queueName,
			basicProperties: properties,
			body: body);
	}

	/// <summary>
	/// Reads a message from RabbitMQ.
	/// </summary>
	/// <typeparam name="T">Generic type of message.</typeparam>
	/// <param name="queueName">Queue where is message is read from.</param>
	/// <returns></returns>
	public T? ReadMessage<T>(string queueName) where T : class
	{
		IModel channel = connection.GetChannel();

		DeclareQueue(channel, queueName);

		BasicGetResult data = channel.BasicGet(queue: queueName, autoAck: false);

		if (data is null)
		{
			return null;
		}

		T? message = ConvertToObject<T>(data.Body);

		channel.BasicAck(deliveryTag: data.DeliveryTag, multiple: false);

		return message;
	}

	/// <summary>
	/// Attaches an consumer to RabbitMQ.
	/// </summary>
	/// <typeparam name="T">Generic type of message.</typeparam>
	/// <param name="onMessageReceived">Callback to execute when message is received.</param>
	/// <param name="queueName">Queue to attach consumer to.</param>
	public void Subscribe<T>(Action<T?> onMessageReceived, string queueName) where T : class
	{
		IModel channel = connection.GetChannel();

		DeclareQueue(channel, queueName);

		var consumer = new EventingBasicConsumer(channel);

		consumer.Received += (_, @event) =>
		{
			T? message = ConvertToObject<T>(@event.Body);
			onMessageReceived(message);
			channel.BasicAck(deliveryTag: @event.DeliveryTag,
								multiple: false);
		};
		channel.BasicConsume(queue: queueName,
					 autoAck: false,
					 consumer: consumer);
	}

	/// <summary>
	/// Converts a generic type to a byte array.
	/// </summary>
	/// <typeparam name="T">Generic type of message.</typeparam>
	/// <param name="message">Message to convert.</param>
	/// <returns>message in bytes</returns>
	private static byte[] ConvertToBytes<T>(T message) where T : class
	{
		string jsonString = JsonSerializer.Serialize(message);
		return Encoding.UTF8.GetBytes(jsonString);
	}

	/// <summary>
	/// Converts a byte array to an Object
	/// </summary>
	/// <typeparam name="T">Generic type to convert object.</typeparam>
	/// <param name="bytes">Bytes to convert</param>
	/// <returns>A message</returns>
	private static T? ConvertToObject<T>(ReadOnlyMemory<Byte> bytes) where T : class
	{
		byte[] messageBytes = bytes.ToArray();
		string messageString = Encoding.UTF8.GetString(messageBytes);
		return JsonSerializer.Deserialize<T>(messageString);
	}

	/// <summary>
	/// Declares a RabbitMQ queue
	/// </summary>
	/// <param name="channel">Channel connection.</param>
	/// <param name="queueName">Queue name to be declared.</param>
	private static void DeclareQueue(IModel channel, string queueName)
	{
		channel.QueueDeclare(queue: queueName,
			durable: true,
			exclusive: false,
			autoDelete: false,
			arguments: null);
	}
}
