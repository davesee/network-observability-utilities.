using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace EFR.NetworkObservability.RabbitMQ.Test;

[ExcludeFromCodeCoverage]
public class RabbitMQMessengerTest
{
	private readonly IOptions<ConnectionDetails> connectionDetails =
	 Options.Create(
		new ConnectionDetails
		{
			ConnectionString = "localhost",
			UserName = "test",
			Password = "****"
		});

	private readonly RabbitMQConnection rabbitMQConnection;
	private readonly Mock<IConnectionFactory> connectionFactoryMock;
	private readonly Mock<IConnection> connectionMock;
	private readonly Mock<IModel> channelMock;


	public RabbitMQMessengerTest()
	{
		connectionFactoryMock = new Mock<IConnectionFactory>();
		connectionMock = new Mock<IConnection>();
		channelMock = new Mock<IModel>();
		rabbitMQConnection = new RabbitMQConnection(connectionDetails, connectionFactoryMock.Object);
	}

	[Fact]
	public void TestNullConnectionDetails()
	{
		var exception = Assert.Throws<ArgumentNullException>(() => new RabbitMQMessenger(connectionDetails: null));
		Assert.Equal("Value cannot be null. (Parameter 'connectionDetails')", exception.Message);
	}

	[Fact]
	public void TestNullConnection()
	{
		var exception = Assert.Throws<ArgumentNullException>(() => new RabbitMQMessenger(connection: null));
		Assert.Equal("Value cannot be null. (Parameter 'connection')", exception.Message);
	}

	[Fact]
	public void ShouldCreateInstanceWithValidParamsAndNullFactory()
	{
		var rabbitMQMessenger = new RabbitMQMessenger(connectionDetails);
		Assert.NotNull(rabbitMQMessenger);
	}

	[Fact]
	public void ShouldPublishMessage()
	{
		connectionFactoryMock?.Setup(x => x.CreateConnection()).Returns(connectionMock!.Object);
		connectionMock?.Setup(x => x.CreateModel()).Returns(channelMock!.Object);
		channelMock?.Setup(x => x.CreateBasicProperties()).Returns(new Mock<IBasicProperties>().Object).Verifiable();

		var rabbitMQMessenger = new RabbitMQMessenger(rabbitMQConnection!);
		rabbitMQMessenger.PushMessage("testing", "TestQueue");

		Mock.Verify(channelMock, connectionMock, connectionFactoryMock);
		channelMock!.Verify(x => x.BasicPublish(
			It.Is<string>(s => s.Equals("")),
			It.Is<string>(s => s.Equals("TestQueue")),
			It.IsAny<bool>(),
			It.IsAny<IBasicProperties>(),
			It.IsAny<ReadOnlyMemory<Byte>>()
			), Times.Once());
	}

	[Fact]
	public void ShouldReturnNullWhenNoMessagesInQueue()
	{
		BasicGetResult result = null;

		connectionFactoryMock!.Setup(x => x.CreateConnection()).Returns(connectionMock!.Object);
		connectionMock.Setup(x => x.CreateModel()).Returns(channelMock!.Object);
		channelMock.Setup(x => x.BasicGet(
			It.Is<string>(s => s.Equals("TestQueue")),
			It.IsAny<bool>()
			)).Returns(result!).Verifiable();

		var rabbitMQMessenger = new RabbitMQMessenger(rabbitMQConnection!);
		var message = rabbitMQMessenger.ReadMessage<string>("TestQueue");

		Mock.Verify(channelMock, connectionMock, connectionFactoryMock);
		Assert.Null(message);
	}

	[Fact]
	public void ShouldReadMessage()
	{
		string messageBody = "{ message: \"testing\" }";
		BasicGetResult result = new(
				deliveryTag: 100,
				redelivered: false,
				exchange: "Test_Exchange",
				routingKey: "Test_Queue",
				messageCount: 1,
				basicProperties: null,
				body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageBody))
				);
		connectionFactoryMock!.Setup(x => x.CreateConnection()).Returns(connectionMock!.Object);
		connectionMock.Setup(x => x.CreateModel()).Returns(channelMock!.Object);
		channelMock.Setup(x => x.BasicGet(
			It.Is<string>(s => s.Equals("TestQueue")),
			It.IsAny<bool>()
			)).Returns(result).Verifiable();

		var rabbitMQMessenger = new RabbitMQMessenger(rabbitMQConnection!);
		var message = rabbitMQMessenger.ReadMessage<string>("TestQueue");

		Mock.Verify(channelMock, connectionMock, connectionFactoryMock);
		Assert.Equal(message, messageBody);
	}

	[Fact]
	public void ShouldSubscribeToQueue()
	{
		connectionFactoryMock!.Setup(x => x.CreateConnection()).Returns(connectionMock!.Object);
		connectionMock.Setup(x => x.CreateModel()).Returns(channelMock!.Object);

		var rabbitMQMessenger = new RabbitMQMessenger(rabbitMQConnection!);
		string message = "";
		rabbitMQMessenger.Subscribe<string>((queueMessage) =>
		{
			message = queueMessage!;
		}, "TestQueue");

		Mock.Verify(channelMock, connectionMock, connectionFactoryMock);
		//TODO: Figure out how to fire EventHandler for RabbitMQ with the messageBody.
		//Assert.AreEqual(message, messageBody);
	}
}
