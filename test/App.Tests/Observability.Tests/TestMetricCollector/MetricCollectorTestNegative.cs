using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using Xunit;

namespace EFR.NetworkObservability.ApplicationObservability.TestMetricCollector;

[ExcludeFromCodeCoverage]
public class MetricCollectorTestNegative
{
	private readonly string counterName;
	private readonly string histogramName;

	public MetricCollectorTestNegative()
	{
		counterName = "counterName";
		histogramName = "histogramName";
	}

	[Fact]
	public void TestNullInputMeterName()
	{
		var exception = Assert.Throws<ArgumentNullException>(() => MetricCollector.CreateMeter(null));
		Assert.Equal("Value cannot be null. (Parameter 'meterName')", exception.Message);
	}

	[Fact]
	public void TestNullInputMeterCounter()
	{
		var exception = Assert.Throws<ArgumentNullException>(() => MetricCollector.CreateCounter(null, counterName, null));
		Assert.Equal("Value cannot be null. (Parameter 'meter')", exception.Message);
	}

	[Fact]
	public void TestEmptyDescInputMeterCounter()
	{
		Meter m = MetricCollector.CreateMeter("meter");
		var exception = Assert.Throws<ArgumentException>(() => MetricCollector.CreateCounter(m, counterName, ""));
		Assert.Equal("Counter description is empty", exception.Message);
	}

	[Fact]
	public void TestNullInputMeterHistogram()
	{
		var exception = Assert.Throws<ArgumentNullException>(() => MetricCollector.CreateHistogram(null, histogramName, ""));
		Assert.Equal("Value cannot be null. (Parameter 'meter')", exception.Message);
	}

	[Fact]
	public void TestEmptyDescInputHistogramCounter()
	{
		Meter m = MetricCollector.CreateMeter("meter");
		var exception = Assert.Throws<ArgumentException>(() => MetricCollector.CreateHistogram(m, counterName, ""));
		Assert.Equal("Histogram description is empty", exception.Message);
	}

	[Fact]
	public void TestNullMetricProviderInputMetricServerName()
	{
		var exception = Assert.Throws<ArgumentNullException>(() => MetricCollector.CreateMeterProvider(null));
		Assert.Equal("Value cannot be null. (Parameter 'meterName')", exception.Message);
	}

	[Fact]
	public void TestBadMetricProviderInputHostname()
	{
		string fakeHostName = "bad-hostname-:1234";
		string validMeterName = "validMeterName";

		var exception = Assert.Throws<ArgumentException>(() => MetricCollector.CreateMeterProvider(validMeterName, fakeHostName));
		Assert.Equal("Input hostname is not valid", exception.Message);
	}


	[Fact]
	public void TestNullHistogramRecord()
	{
		Histogram<int> nullHistogram = null!;

		MetricCollector.RecordHistogram(nullHistogram);
		Assert.Null(nullHistogram);
	}

	[Fact]
	public void TestNullCounterInc()
	{
		Counter<int> nullCounter = null!;

		MetricCollector.IncCounter(nullCounter);
		Assert.Null(nullCounter);
	}

	[Fact]
	public void TestLockNullMeter()
	{
		Meter nullMeter = null!;

		MetricCollector.LockMeter(nullMeter);
		Assert.Null(nullMeter);
	}

}
