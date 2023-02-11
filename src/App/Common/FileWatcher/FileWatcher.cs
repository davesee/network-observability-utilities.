using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace EFR.NetworkObservability.Common.FileWatcher;

/// <summary>
/// Base class for FileWatchers
/// </summary>
public abstract class FileWatcher
{
	/// <summary>
	/// Event invoked when file is created
	/// </summary>
	public Action<string>? OnFileCreated;

	/// <summary>
	/// Directory path to watch
	/// </summary>
	protected readonly string pathToWatch;

	/// <summary>
	/// Extensions to watch for
	/// </summary>
	protected readonly ImmutableArray<string> fileFilters;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="pathToWatch">Directory to watch for files</param>
	/// <param name="fileFilter">filter for files watched</param>
	public FileWatcher(string pathToWatch, string fileFilter)
		: this(pathToWatch, new string[] { fileFilter })
	{
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="pathToWatch"></param>
	/// <param name="fileFilters"></param>
	public FileWatcher(string pathToWatch, IList<string> fileFilters)
		: this(pathToWatch, fileFilters.Where(x => !string.IsNullOrEmpty(x)).ToImmutableArray())
	{
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="pathToWatch"></param>
	/// <param name="fileFilters"></param>
	public FileWatcher(string pathToWatch, string[] fileFilters)
		: this(pathToWatch, fileFilters.ToList())
	{
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="pathToWatch"></param>
	/// <param name="fileFilters"></param>
	public FileWatcher(string pathToWatch, ImmutableArray<string> fileFilters)
	{
		ArgumentNullException.ThrowIfNull(pathToWatch);
		ArgumentNullException.ThrowIfNull(fileFilters);
		if (fileFilters.IsEmpty) throw new ArgumentNullException(nameof(fileFilters));


		this.pathToWatch = pathToWatch;
		this.fileFilters = fileFilters;
	}

	/// <summary>
	/// Virtual method that invokes event when file is created.
	/// </summary>
	/// <param name="path">Path to where file was created</param>
	protected virtual void InvokeOnFileCreated(string path) => OnFileCreated!.Invoke(path);
}
