using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EFR.NetworkObservability.Common.Exceptions;

/// <summary>
/// Custom Exception
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class ConfigurationException : Exception
{
	/// <summary>
	/// Default Constructor
	/// </summary>
	public ConfigurationException()
	{
	}
	/// <summary>
	/// Builds exception with message
	/// </summary>
	/// <param name="message">error message</param>
	public ConfigurationException(string message) : base(message)
	{
	}

	/// <summary>
	/// Builds exception with message and inner exception
	/// </summary>
	/// <param name="message">error message</param>
	/// <param name="inner">inner exception</param>
	public ConfigurationException(string message, Exception inner) : base(message, inner)
	{
	}

	/// <summary>
	/// Builds exception with Serialization Info and Streaming Context
	/// </summary>
	/// <param name="info">Serialization Info</param>
	/// <param name="context">exception stream context</param>
	protected ConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}

