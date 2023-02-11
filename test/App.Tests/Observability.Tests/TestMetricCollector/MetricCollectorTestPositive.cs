using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using Xunit;

namespace EFR.NetworkObservability.ApplicationObservability.Test.TestMetricCollector;

[ExcludeFromCodeCoverage]
public class MetricCollectorTestPositive
{
	private readonly string meterName;
	private readonly string counterName;
	private readonly string histogramName;
	private readonly string description;

	public MetricCollectorTestPositive()
	{
		meterName = "meterName";
		counterName = "counterName";
		histogramName = "histogramName";
		description = "description";
	}

	[Fact]
	public void TestCreateCounter()
	{
		Meter meter = MetricCollector.CreateMeter(meterName);
		Counter<int> counter = MetricCollector.CreateCounter(meter, counterName: counterName, description: description);
		Counter<int> counterWithUnit = MetricCollector.CreateCounter(meter, counterName: counterName, unit: "tests", description: description);

		Assert.NotNull(counter);
		Assert.NotNull(counterWithUnit);
		Assert.Equal("tests", counterWithUnit.Unit);
	}

	[Fact]
	public void TestCreateHistogram()
	{
		Meter meter = MetricCollector.CreateMeter(meterName);
		Histogram<int> histogram = MetricCollector.CreateHistogram(meter, histogramName: histogramName, description: description);
		Histogram<int> histogramWithUnit = MetricCollector.CreateHistogram(meter, histogramName: histogramName, unit: "tests", description: description);

		Assert.NotNull(histogram);
		Assert.NotNull(histogramWithUnit);
		Assert.Equal("tests", histogramWithUnit.Unit);
	}

	[Fact]
	public void TestCreateMeterProvider()
	{
		string fakeHostName = "localhost:1234";

		MeterProvider mpDefaultPort = MetricCollector.CreateMeterProvider(meterName);
		MeterProvider mpFakePort = MetricCollector.CreateMeterProvider(meterName, fakeHostName);

		Assert.NotNull(mpDefaultPort);
		Assert.NotNull(mpFakePort);

		string fakeHostNameWildCard = "127.0.0.1:1234";

		MeterProvider mpWildcardFakePort = MetricCollector.CreateMeterProvider(meterName, fakeHostNameWildCard);

		Assert.NotNull(mpWildcardFakePort);
	}


	[Fact]
	public void TestIncCounter()
	{
		Meter meter = MetricCollector.CreateMeter(meterName);
		Counter<int> counter = MetricCollector.CreateCounter(meter, counterName, description: description);
		Counter<int> counter2 = MetricCollector.CreateCounter(meter, counterName, description: description);
		Assert.Equal(counter.Meter, counter2.Meter);

		MetricCollector.IncCounter(counter);
		MetricCollector.IncCounter(counter);
		MetricCollector.IncCounter(counter2, 2);
		Assert.Equal(counter.ToString(), counter2.ToString());
	}

	[Fact]
	public void TestRecordHistogram()
	{
		Meter meter = MetricCollector.CreateMeter(meterName);
		Histogram<int> histogram = MetricCollector.CreateHistogram(meter, histogramName, description: description);
		Histogram<int> histogram2 = MetricCollector.CreateHistogram(meter, histogramName, description: description);
		Assert.Equal(histogram.Meter, histogram2.Meter);

		MetricCollector.RecordHistogram(histogram);
		MetricCollector.RecordHistogram(histogram);
		MetricCollector.RecordHistogram(histogram2, 2);
		Assert.Equal(histogram.ToString(), histogram2.ToString());
	}

	[Fact]
	public void TestLockCounterHistogram()
	{
		Meter meter = MetricCollector.CreateMeter(meterName);
		Histogram<int> histogram = MetricCollector.CreateHistogram(meter, histogramName, description: description);
		Counter<int> counter = MetricCollector.CreateCounter(meter, counterName, description: description);
		Assert.Equal(counter.Meter, histogram.Meter);

		MetricCollector.LockMeter(meter);
		Assert.False(counter.Enabled);
		Assert.False(histogram.Enabled);
	}
}
