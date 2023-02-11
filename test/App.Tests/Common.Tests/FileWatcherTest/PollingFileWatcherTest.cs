using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using Xunit;

namespace EFR.NetworkObservability.Common.FileWatcher.Test;

[ExcludeFromCodeCoverage]
public class PollingFileWatcherTest : IDisposable
{
	private readonly string pathToWatch;

	public PollingFileWatcherTest()
	{
		pathToWatch = Path.Combine(Path.GetTempPath(), "watchpath");
		Directory.CreateDirectory(pathToWatch);
	}

	public void Dispose()
	{
		Directory.Delete(pathToWatch, true);
		GC.SuppressFinalize(this);
	}


	[Fact]
	public void ShouldThrowArgumentNullException_WhenWatchDirIsNull()
	{
		Assert.Throws<ArgumentNullException>(() => new PollingFileWatcher(null));
	}

	[Fact]
	public void ShouldThrowArgumentNullException_WhenFilterIsNull()
	{
		Assert.Throws<ArgumentNullException>(() => new PollingFileWatcher("test", fileFilter: null));
	}


	[Fact]
	public void ShouldThrowDirectoryNotFoundException_WhenDirecoryDoesntExists()
	{
		using var watcher = new PollingFileWatcher("testing");
		Assert.Throws<DirectoryNotFoundException>(() => watcher.StartPolling());
	}

	[Fact]
	public void ShouldDoNothing_WhenEventNotSet()
	{
		using var watcher = new PollingFileWatcher(pathToWatch);
		watcher.StartPolling(interval: 1000);

		Thread.Sleep(1000);
	}

	// [Fact]
	// public void ShouldTriggerEvent_WhenFileDropped()
	// {
	// 	var fileName = "test.txt";
	// 	var eventFileName = "";
	// 	using FileStream file = File.Create(Path.Combine(pathToWatch, fileName));
	// 	byte[] data = Encoding.UTF8.GetBytes("somedata");
	// 	file.Write(data, 0, data.Length);
	// 	file.Flush();
	// 	file.Close();

	// 	var extension = Path.GetExtension(fileName);

	// 	using var watcher = new PollingFileWatcher(pathToWatch, extension);
	// 	watcher.OnFileCreated += (f) => eventFileName = f;
	// 	watcher.StartPolling(interval: 1000);

	// 	Thread.Sleep(1000);
	// 	Assert.EndsWith(fileName, eventFileName);
	// }
}
