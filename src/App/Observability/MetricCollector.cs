using System;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace EFR.NetworkObservability.ApplicationObservability;

/// <summary>
/// Wrapper Class that allows easier service integration with Metric Instrumentation
/// Utilizes System.Diagnostics.Metrics base class
/// </summary>
public static class MetricCollector
{
	private const string DEFAULT_OPEN_TELEMETRY_HOSTNAME = "localhost:9184";
	private const string VALID_HOST_PATTERN = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])[:][0-9]{2,4}$";
	private static readonly Regex validHostnameRegex = new(VALID_HOST_PATTERN);

	/// <summary>
	/// Generates a Meter object that allows creation and updates to counters and histograms
	/// </summary>
	/// <param name="meterName">Name of the meter</param>
	/// <param name="version">Version of the meter</param>
	/// <returns>A System.Diagnostics.Metrics.Meter object</returns>
	public static Meter CreateMeter(string meterName,
																	string version = "1.0.0")
	{
		ArgumentNullException.ThrowIfNull(meterName, nameof(meterName));
		return new Meter(meterName, version);
	}

	/// <summary>
	/// Generates a Counter object that allows creation and updates to metrics
	/// </summary>
	/// <param name="meter">Meter object used to create child Counter</param>
	/// <param name="counterName">Name of the counter</param>
	/// <param name="description">Description of the counter</param>
	/// <param name="unit">Unit of the Counter</param>
	/// <returns>A System.Diagnostics.Metrics.Counter object</returns>
	public static Counter<int> CreateCounter(Meter meter,
																						 string counterName,
																						 string description,
																						 string unit = "count")
	{
		ArgumentNullException.ThrowIfNull(meter, nameof(meter));

		if (string.IsNullOrEmpty(description))
		{
			throw new ArgumentException("Counter description is empty");
		}

		return meter.CreateCounter<int>(name: counterName, unit: unit, description: description);
	}

	/// <summary>
	/// Generates a Histogram object that allows creation and updates to metrics
	/// </summary>
	/// <param name="meter">Meter object used to create child Histogram</param>
	/// <param name="histogramName">Name of the histogram</param>
	/// <param name="description">Description of the histogram</param>
	/// <param name="unit">Unit of the histogram</param>
	/// <returns>A System.Diagnostics.Metrics.Histogram object</returns>
	public static Histogram<int> CreateHistogram(Meter meter,
																								 string histogramName,
																								 string description,
																								 string unit = "count")
	{
		ArgumentNullException.ThrowIfNull(meter, nameof(meter));

		if (string.IsNullOrEmpty(description))
		{
			throw new ArgumentException("Histogram description is empty");
		}


		return meter.CreateHistogram<int>(name: histogramName, unit: unit, description: description);
	}

	/// <summary>
	/// Spins up a server to export the metrics so it can be scraped by Prometheus
	/// </summary>
	/// <param name="meterName">name of the meter that contains metrics to export</param>
	/// <param name="hostname">hostname to serve the metrics</param>
	/// <remarks>/metrics is attached implicitly to the server hostname</remarks>
	public static MeterProvider CreateMeterProvider(string meterName,
																									string hostname = DEFAULT_OPEN_TELEMETRY_HOSTNAME)
	{
		ArgumentNullException.ThrowIfNull(meterName, nameof(meterName));
		Match match = validHostnameRegex.Match(hostname);
		if (match.Success is false)
		{
			throw new ArgumentException("Input hostname is not valid");
		}

		return Sdk.CreateMeterProviderBuilder()
					 .AddMeter(meterName)
					 .AddPrometheusExporter(opt =>
					 {
						 opt.StartHttpListener = true;
						 opt.HttpListenerPrefixes = new string[] { $"http://{hostname}/" };
					 }).Build();
	}

	/// <summary>
	/// Record metric value to a histogram
	/// </summary>
	/// <param name="histogram">histogram object to increment value to</param>
	/// <param name="count">count to record the value by</param>
	/// <remarks>negative increment counts are allowed and will be summed with current value</remarks>
	public static void RecordHistogram(Histogram<int> histogram,
																		 int count = 1)
	{
		histogram?.Record(count);
	}

	/// <summary>
	/// Increment metric value to a counter
	/// </summary>
	/// <param name="counter">counter object to increment value to</param>
	/// <param name="count">count to increment the value by</param>
	/// <remarks>negative increment counts are allowed</remarks>
	public static void IncCounter(Counter<int> counter,
																int count = 1)
	{
		counter?.Add(count);
	}

	/// <summary>
	/// Locks the meter object and disable all child metric value updates
	/// </summary>
	/// <param name="meter">Meter object to lock</param>
	/// <remarks>Once locked, child metrics can no longer be re-enabled, a new meter must be created</remarks>
	public static void LockMeter(Meter meter)
	{
		meter?.Dispose();
	}
}
