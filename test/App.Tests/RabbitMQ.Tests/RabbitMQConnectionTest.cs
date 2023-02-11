using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace EFR.NetworkObservability.RabbitMQ.Test;

[ExcludeFromCodeCoverage]
public class RabbitMQConnectionTest
{
	private readonly IOptions<ConnectionDetails> connectionDetails =
	 Options.Create(
		new ConnectionDetails
		{
			ConnectionString = "localhost",
			UserName = "test",
			Password = "****"
		});

	private readonly Mock<IConnectionFactory> connectionFactoryMock;
	private readonly Mock<IConnection> connectionMock;
	private readonly Mock<IModel> channelMock;

	public RabbitMQConnectionTest()
	{
		connectionFactoryMock = new Mock<IConnectionFactory>();
		connectionMock = new Mock<IConnection>();
		channelMock = new Mock<IModel>();
	}

	[Fact]
	public void TestNullConnectionDetails()
	{
		var exception = Assert.Throws<ArgumentNullException>(() => new RabbitMQConnection(null));
		Assert.Equal("Value cannot be null. (Parameter 'connectionDetails')", exception.Message);
	}

	[Fact]
	public void ShouldCreateInstanceWithValidParamsAndNullFactory()
	{
		using var rabbitMQConnection = new RabbitMQConnection(connectionDetails, null);
		Assert.NotNull(rabbitMQConnection);
	}

	[Fact]
	public void ShouldCreateNewConnectionWhenConnectionNull()
	{
		connectionFactoryMock!.Setup(x => x.CreateConnection()).Returns(connectionMock!.Object);

		using var rabbitMQConnection = new RabbitMQConnection(connectionDetails, connectionFactoryMock.Object);

		var connection = rabbitMQConnection.GetConnection();

		Assert.NotNull(connection);

		connectionFactoryMock.Verify(x => x.CreateConnection(), Times.Once());
	}

	[Fact]
	public void ShouldCreateNewConnectionWhenConnectionClosed()
	{
		connectionMock!.Setup(x => x.IsOpen).Returns(false);
		connectionFactoryMock!.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

		using var rabbitMQConnection = new RabbitMQConnection(connectionDetails, connectionFactoryMock.Object);

		var connection = rabbitMQConnection.GetConnection();
		Assert.NotNull(connection);

		connection = rabbitMQConnection.GetConnection();
		Assert.NotNull(connection);

		connectionFactoryMock.Verify(x => x.CreateConnection(), Times.Exactly(2));
	}

	[Fact]
	public void ShouldReuseConnectionWhenConnectionOpen()
	{
		connectionMock!.Setup(x => x.IsOpen).Returns(true);
		connectionFactoryMock!.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

		using var rabbitMQConnection = new RabbitMQConnection(connectionDetails, connectionFactoryMock.Object);

		var connection = rabbitMQConnection.GetConnection();
		Assert.NotNull(connection);

		connection = rabbitMQConnection.GetConnection();
		Assert.NotNull(connection);

		connectionFactoryMock.Verify(x => x.CreateConnection(), Times.Once());
	}

	[Fact]
	public void ShouldCreateNewChannelWhenChannelIsNull()
	{
		connectionMock!.Setup(x => x.CreateModel()).Returns(channelMock!.Object);
		connectionFactoryMock!.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

		using var rabbitMQConnection = new RabbitMQConnection(connectionDetails, connectionFactoryMock.Object);

		var channel = rabbitMQConnection.GetChannel();

		Assert.NotNull(channel);

		connectionMock.Verify(x => x.CreateModel(), Times.Once());
	}

	[Fact]
	public void ShouldCreateNewChannelWhenChannelIsClosed()
	{
		channelMock!.SetupGet(x => x.IsOpen).Returns(false);
		connectionMock!.Setup(x => x.CreateModel()).Returns(channelMock.Object);
		connectionFactoryMock!.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

		using var rabbitMQConnection = new RabbitMQConnection(connectionDetails, connectionFactoryMock.Object);

		var channel = rabbitMQConnection.GetChannel();
		Assert.NotNull(channel);

		channel = rabbitMQConnection.GetChannel();
		Assert.NotNull(channel);

		connectionMock.Verify(x => x.CreateModel(), Times.Exactly(2));
	}

	[Fact]
	public void ShouldReuseChannelWhileChannelStillOpen()
	{
		channelMock!.SetupGet(x => x.IsOpen).Returns(true);
		connectionMock!.Setup(x => x.CreateModel()).Returns(channelMock.Object);
		connectionFactoryMock!.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

		using var rabbitMQConnection = new RabbitMQConnection(connectionDetails, connectionFactoryMock.Object);

		var channel = rabbitMQConnection.GetChannel();
		Assert.NotNull(channel);

		channel = rabbitMQConnection.GetChannel();
		Assert.NotNull(channel);

		connectionMock.Verify(x => x.CreateModel(), Times.Once());
	}
}
