using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using EFR.NetworkObservability.DataModel.Contexts;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFR.NetworkObservability.DataModel.Services.Test;

[ExcludeFromCodeCoverage]
public class PcapServiceTest : IDisposable
{
	private readonly DbContextOptions<PcapContext> options =
		new DbContextOptionsBuilder<PcapContext>()
		.UseInMemoryDatabase(Guid.NewGuid().ToString())
		.Options;

	private readonly PcapContext context;

	private readonly PcapService service;

	public PcapServiceTest()
	{
		context = new PcapContext(options);
		service = new PcapService(context);
	}

	public void Dispose()
	{
		context.Database.EnsureDeleted();
		context.Dispose();
		GC.SuppressFinalize(this);
	}

	[Fact]
	public async Task ShouldBulkPcapIndices()
	{
		var pcapIndices = new Faker<PcapIndex>().Generate(3);

		await service.BulkInsertPcapIndicesAsync(pcapIndices);

		var actual = context.PcapIndices.ToList();

		Assert.Equal(pcapIndices, actual);
		//pcapIndices.ForEach(p => Assert.Contains(p, actual));
	}

	[Fact]
	public async Task ShouldInsertPcapIndices()
	{
		var pcapIndices = new Faker<PcapIndex>().Generate(3);

		await service.InsertPcapIndicesAsync(pcapIndices);

		var actual = context.PcapIndices.ToList();

		Assert.Equal(pcapIndices, actual);
		//pcapIndices.ForEach(p => Assert.Contains(p, actual));
	}

	[Fact]
	public async Task ShouldInsertJobProcessingLog()
	{
		var jobLog = new Faker<PcapFileProcessingLog>().Generate();

		await service.InsertJobProcessingLogAsync(jobLog);

		var actualJobLog = context.PcapFileProcessingLogs.First();

		Assert.Equal(jobLog, actualJobLog);
	}

	[Fact]
	public async Task ShouldGetJobProcessingLog()
	{

		var id = Guid.NewGuid();

		PcapFileProcessingLog expectedLog = new Faker<PcapFileProcessingLog>()
			.RuleFor(d => d.ProcessingJobID, f => id)
			.Generate();

		context.Add(expectedLog);
		context.SaveChanges();

		var actualLog = await service.GetJobProcessingLogAsync(id);

		Assert.Equal(expectedLog, actualLog);
	}

	[Fact]
	public async Task ShouldUpdateJobStatus()
	{
		var id = Guid.NewGuid();

		PcapFileProcessingLog jobLog = new Faker<PcapFileProcessingLog>()
			.RuleFor(d => d.ProcessingJobID, f => id)
			.RuleFor(d => d.ProcessingJobStatus, f => JobStatus.InProgress)
			.Generate();

		context.Add(jobLog);
		context.SaveChanges();

		await service!.UpdateJobStatusAsync(id, JobStatus.Completed);

		var updatedJobLog = context.PcapFileProcessingLogs
			.Where(f => f.ProcessingJobID == id)
			.First();

		Assert.Equal(JobStatus.Completed, updatedJobLog.ProcessingJobStatus);
	}

	[Fact]
	public async Task ShouldUpdateNotes()
	{
		var notes = "testing";

		var id = Guid.NewGuid();

		PcapFileProcessingLog jobLog = new Faker<PcapFileProcessingLog>()
			.RuleFor(d => d.ProcessingJobID, f => id)
			.RuleFor(d => d.Notes, f => "")
			.Generate();

		context.Add(jobLog);
		context.SaveChanges();

		await service!.UpdateNotesAsync(id, notes);

		var updatedJobLog = context.PcapFileProcessingLogs
			.Where(f => f.ProcessingJobID == id)
			.First();

		Assert.Equal(updatedJobLog.Notes, notes);
	}

	[Fact]
	public async Task ShouldInsertPcapMetaData()
	{
		var pcapMetaData = new Faker<PcapMetaData>().Generate();

		await service.InsertPcapMetaDataAsync(pcapMetaData);

		var insertedPcapMetaData = context.PcapMetaDatas.First();

		Assert.Equal(pcapMetaData, insertedPcapMetaData);
	}

	[Fact]
	public async Task ShouldInsertEventMetaData()
	{
		var eventMetaData = new Faker<EventMetaData>().Generate();

		await service.InsertEventMetaDataAsync(eventMetaData);

		var count = context.EventMetaDatas.Count();

		Assert.Equal(1, count);
	}

}
