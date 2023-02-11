using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using EFR.NetworkObservability.Common.Logging;
using Serilog;

namespace EFR.NetworkObservability.Common.FileWatcher;

/// <summary>
/// Watches files using Polling
/// </summary>
public class PollingFileWatcher : FileWatcher, IDisposable
{
	private long interval;
	private bool alreadyDisposed;

	private readonly ILogger logger;

	private readonly Timer timer;
	private readonly List<string> filesInProcess;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="pathToWatch">Path where to watch files</param>
	/// <param name="fileFilter">Filter for files to watch</param>
	public PollingFileWatcher(string pathToWatch, string fileFilter = "*.*")
		: this(pathToWatch, new List<string> { fileFilter })
	{
	}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="pathToWatch">Path where to watch files</param>
	/// <param name="fileFilters">Filters for files to watch</param>
	public PollingFileWatcher(string pathToWatch, List<string> fileFilters)
		: base(pathToWatch, fileFilters)
	{
		logger = SerilogLogger.instance;

		timer = new Timer(new TimerCallback(Handler));
		filesInProcess = new List<string>();
	}

	/// <summary>
	/// Starts the polling processes
	/// </summary>
	/// <param name="interval">Interval(in ms) at which files are polled. Defaults to 30secs</param>
	/// <exception cref="DirectoryNotFoundException">Thrown if watch directory doesn't exist</exception>
	public void StartPolling(long interval = 30000)
	{
		if (Directory.Exists(pathToWatch) is false)
		{
			throw new DirectoryNotFoundException($"The specified watch directory({pathToWatch}) does not exist!");
		}

		logger.Information($"Checking directory({pathToWatch}) for files every {interval} ms!");

		this.interval = interval;
		timer.Change(0, interval);
	}

	/// <summary>
	/// Stops the polling processes
	/// </summary>
	public void StopPolling() => timer.Change(Timeout.Infinite, Timeout.Infinite);

	private void Handler(object? state)
	{
		logger.Debug($"Polling event: {DateTime.Now:h:mm:ss.fff}");

		StopPolling();

		try
		{
			var files = MultiEnumerateFiles();

			filesInProcess.RemoveAll(f => files.Contains(f) is false);
			files = files.Where(f => filesInProcess.Contains(f) is false);

			foreach (var file in files)
			{
				if (Utils.FileAvailable(file) is true)
				{
					filesInProcess.Add(file);
					logger.Information($"File Found: {file}");
					InvokeOnFileCreated(file);
				}
			}
		}
		catch (Exception ex)
		{
			logger.Error(ex, $"Error found while processing file!");
		}
		finally
		{
			timer.Change(interval, interval); // Starts time delayed.
		}
	}

	private IEnumerable<string> MultiEnumerateFiles()
	{
		foreach (var pattern in fileFilters)
			foreach (var file in Directory.EnumerateFiles(pathToWatch, pattern))
				yield return file;
	}

	/// <summary>
	/// Disposes of resources
	/// </summary>
	public void Dispose()
	{
		if (alreadyDisposed)
			return;

		timer.Dispose();
		alreadyDisposed = true;
		GC.SuppressFinalize(this);
	}
}
