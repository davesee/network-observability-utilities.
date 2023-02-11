using System;
using System.IO;
using EFR.NetworkObservability.Common.Exceptions;
using PacketDotNet;

namespace EFR.NetworkObservability.Common;

/// <summary>
/// Static Utility class
/// </summary>
public static class Utils
{
	/// <summary>
	/// Retrieves environment variable value.
	/// </summary>
	/// <param name="name">Name of environment variable.</param>
	/// <returns>Value from environment.</returns>
	/// <exception cref="EnvironmentVariableNotSetException">Thrown if variable doesn't exist or is empty</exception>
	public static string GetEnvVar(string name) => GetEnvVar<string>(name);

	/// <summary>
	/// Retrieves environment variable value.
	/// </summary>
	/// <typeparam name="T">Type of variable stored in environment.</typeparam>
	/// <param name="name">Name of environment variable.</param>
	/// <returns>Value from environment.</returns>
	/// <exception cref="EnvironmentVariableNotSetException">Thrown if variable doesn't exist or is empty</exception>
	public static T GetEnvVar<T>(string name)
	{
		string? value = Environment.GetEnvironmentVariable(name);
		EnvironmentVariableNotSetException.ThrowIfNullOrEmpty(value);
		return (T)Convert.ChangeType(value, typeof(T));
	}

	/// <summary>
	/// Retrieves environment variable value or default.
	/// </summary>
	/// <typeparam name="T">Type of variable stored in environment.</typeparam>
	/// <param name="name">Name of environment variable.</param>
	/// <param name="defaultValue">Default value.</param>
	/// <returns>Value from environment.</returns>
	/// <exception cref="EnvironmentVariableNotSetException">Thrown if variable doesn't exist or is empty</exception>
	public static T GetEnvVarOrDefault<T>(string name, T defaultValue)
	{
		string? value = Environment.GetEnvironmentVariable(name);

		if (string.IsNullOrEmpty(value))
		{
			return defaultValue;
		}
		else
		{
			return (T)Convert.ChangeType(value, typeof(T));
		}
	}

	/// <summary>
	/// Creates directory if doesn't exist
	/// </summary>
	/// <param name="directoryName">Directory name to create</param>
	/// <param name="deleteExisting">Delete existing directory</param>
	public static void CreateDirectory(string directoryName, bool deleteExisting = false)
	{
		if (Directory.Exists(directoryName) is false)
		{
			Directory.CreateDirectory(directoryName);
		}
		else if (deleteExisting is true)
		{
			Directory.Delete(directoryName, true);
			Directory.CreateDirectory(directoryName);
		}
	}

	/// <summary>
	/// Deletes file if it exists
	/// </summary>
	/// <param name="fileName">Filename to create</param>
	public static void DeleteFile(string fileName)
	{
		if (File.Exists(fileName) is true)
		{
			File.Delete(fileName);
		}
	}

	/// <summary>
	/// Checks if a file is being used by a process
	/// </summary>
	/// <param name="fileName">Name of file to check</param>
	/// <returns>If a file is available</returns>
	public static bool FileAvailable(string fileName)
	{
		try
		{
			using var file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
			return true;
		}
		catch (IOException)
		{
			return false;
		}
	}


	/// <summary>
	/// Gets traffic type
	/// </summary>
	/// <param name="sourcePort">Source Port</param>
	/// <param name="destPort">Destination Port</param>
	/// <param name="protocol">Protocol</param>
	/// <returns></returns>
	public static string? GetTrafficType(int? sourcePort, int? destPort, ProtocolType protocol)
	{
		string? value = null;
		if (sourcePort != null && destPort != null)
		{
			TrafficTypes.values.TryGetValue((Math.Min((int)sourcePort, (int)destPort), protocol), out value);
		}
		return value;
	}

	/// <summary>
	/// Retrieves a prefix for logging
	/// </summary>
	/// <param name="jobId">Current Job Id</param>
	/// <returns>Prefix for logging</returns>
	public static string GetLoggingPrefix(string jobId) => $"{Constants.JOB_ID_LOG_PREFIX} {jobId} -";
}
