using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace EFR.NetworkObservability.Common.Exceptions;

/// <summary>
/// Thrown when RabbitMQ message is invalid
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class InvalidRabbitMQMessageException : Exception
{
	/// <summary>
	/// Instantiate with no params
	/// </summary>
	public InvalidRabbitMQMessageException()
	{
	}

	/// <summary>
	/// Instantiate with message
	/// </summary>
	public InvalidRabbitMQMessageException(string message) : base(message) { }

	/// <summary>
	/// Instantiate with message and inner exception
	/// </summary>
	public InvalidRabbitMQMessageException(string message, Exception inner) : base(message, inner) { }

	/// <summary>
	/// Instantiate with message and info and context
	/// </summary>
	protected InvalidRabbitMQMessageException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	/// <summary>
	/// Throws exception if rabbitMQ message is null
	/// </summary>
	/// <param name="message">message to check</param>
	/// <exception cref="InvalidRabbitMQMessageException">Exception Thrown</exception>
	public static void ThrowIfMessageNull([NotNull] object message)
	{
		if (message is null)
		{
			throw new InvalidRabbitMQMessageException("The incoming RabbitMQ message is empty!");
		}
	}

	/// <summary>
	/// Throws exception if argument is null
	/// </summary>
	/// <param name="argument">value to check</param>
	/// <param name="paramName">name of argument</param>
	/// <exception cref="InvalidRabbitMQMessageException">Exception thrown</exception>
	public static void ThrowIfNull([NotNull] object argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		if (argument is null)
		{
			throw new InvalidRabbitMQMessageException($"The incoming RabbitMQ message does not contain {paramName}");
		}
	}

	/// <summary>
	/// Throws exception if arument is null or empty
	/// </summary>
	/// <param name="argument">String value to check</param>
	/// <param name="paramName">name of argument</param>
	public static void ThrowIfNullOrEmpty([NotNull] string argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		if (string.IsNullOrEmpty(argument) is true)
		{
			throw new InvalidRabbitMQMessageException($"The incoming RabbitMQ message does not contain {paramName}");
		}
	}

}
