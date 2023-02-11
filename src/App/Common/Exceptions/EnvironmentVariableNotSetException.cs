using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace EFR.NetworkObservability.Common.Exceptions;

/// <summary>
/// This is exception thrown when an environment variable is not set.
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class EnvironmentVariableNotSetException : Exception
{
	private const string DEFAULT_MESSAGE = "Environment variable must be set!";

	/// <summary>
	/// Default Constructor
	/// </summary>
	public EnvironmentVariableNotSetException() : base(DEFAULT_MESSAGE)
	{
	}
	/// <summary>
	/// Builds exception with message
	/// </summary>
	/// <param name="message">error message</param>
	public EnvironmentVariableNotSetException(string message) : base(message)
	{
	}

	/// <summary>
	/// Builds exception with message and inner exception
	/// </summary>
	/// <param name="message">error message</param>
	/// <param name="inner">inner exception</param>
	public EnvironmentVariableNotSetException(string message, Exception inner) : base(message, inner)
	{
	}

	/// <summary>
	/// Builds exception with Serialization Info and Streaming Context
	/// </summary>
	/// <param name="info">Serialization Info</param>
	/// <param name="context">exception stream context</param>
	protected EnvironmentVariableNotSetException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	/// <summary>
	/// Throws an EnvironmentVariableNotSetException if argument is null.
	/// </summary>
	/// <param name="argument">The reference type argument to validate as non-null.</param>
	/// <param name="paramName">The name of the parameter with which argument corresponds.</param>
	/// <exception cref="EnvironmentVariableNotSetException">Argument is null</exception>
	public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = default)
	{
		if (argument is null)
		{
			string message = $"Environment variable {paramName} is null! {DEFAULT_MESSAGE}";
			throw new EnvironmentVariableNotSetException(message);
		}
	}


	/// <summary>
	/// Throws an EnvironmentVariableNotSetException if argument is null.
	/// </summary>
	/// <param name="argument">The reference type argument to validate as non-null.</param>
	/// <param name="paramName">The name of the parameter with which argument corresponds.</param>
	/// <exception cref="EnvironmentVariableNotSetException">Argument is null</exception>
	public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression("argument")] string? paramName = default)
	{
		if (string.IsNullOrEmpty(argument) is true)
		{
			string message = $"Environment variable {paramName} is null! {DEFAULT_MESSAGE}";
			throw new EnvironmentVariableNotSetException(message);
		}
	}
}
