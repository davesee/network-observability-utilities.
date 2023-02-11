using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using EFR.NetworkObservability.DataModel.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EFR.NetworkObservability.DataModel.Services;

/// <summary>
/// Service for interacting with DV ef context
/// </summary>
public class PcapService : ServiceBase
{
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="context">Pcapcontext object</param>
	public PcapService(PcapContext context) : base(context)
	{
	}

	/// <summary>
	/// Bulk inserts pcap indices into db
	/// </summary>
	/// <param name="pcapIndices">indices to insert</param>
	/// <returns>Awaitable task</returns>
	[ExcludeFromCodeCoverage]
	public async Task BulkInsertPcapIndicesAsync(IList<PcapIndex> pcapIndices)
	{
		await BulkCreateAsync(pcapIndices);
		await SaveAsync();
	}

	/// <summary>
	/// Inserts pcap indices into db
	/// </summary>
	/// <param name="pcapIndices">indices to insert</param>
	/// <returns>Awaitable task</returns>
	public async Task InsertPcapIndicesAsync(IList<PcapIndex> pcapIndices)
	{
		await CreateEntitiesAsync(pcapIndices);
		await SaveAsync();
	}


	/// <summary>
	/// Inserts PcapFileProcessingLog into db
	/// </summary>
	/// <param name="jobprocessinglog">Object to insert</param>
	/// <returns>PcapFileProcessingLog id</returns>
	public async Task<long> InsertJobProcessingLogAsync(PcapFileProcessingLog jobprocessinglog)
	{
		await CreateAsync(jobprocessinglog);
		await SaveAsync();
		return jobprocessinglog.PcapFileProcessingLogID;
	}

	/// <summary>
	/// Inserts PcapFileProcessingLog into db
	/// </summary>
	/// <param name="jobprocessinglog">Object to insert</param>
	/// <returns>PcapFileProcessingLog id</returns>
	public long InsertJobProcessingLog(PcapFileProcessingLog jobprocessinglog)
	{
		Create(jobprocessinglog);
		Save();
		return jobprocessinglog.PcapFileProcessingLogID;
	}

	/// <summary>
	/// Retrieves PcapFileProcessingLog by id
	/// </summary>
	/// <param name="processingJobID">Id of PcapFileProcessingLog to get</param>
	/// <returns>PcapFileProcessingLog object</returns>
	public async Task<PcapFileProcessingLog> GetJobProcessingLogAsync(Guid processingJobID)
	{
		var jp = await (
				from l in QueryAll<PcapFileProcessingLog>()
				where l.ProcessingJobID == processingJobID
				select l
			).SingleAsync();

		return jp;
	}

	/// <summary>
	/// Updates job status
	/// </summary>
	/// <param name="processingJobID">job id to update status of</param>
	/// <param name="jobStatus">Job Status</param>
	/// <returns>Awaitable task</returns>
	public async Task UpdateJobStatusAsync(Guid processingJobID, JobStatus jobStatus)
	{
		var PcapFileProcessingLog = (
			from l in QueryAll<PcapFileProcessingLog>()
			where l.ProcessingJobID == processingJobID
			select l
			).Single();

		if (JobStatus.Completed == jobStatus)
		{
			PcapFileProcessingLog.EndDateTime = DateTime.UtcNow;
		}

		PcapFileProcessingLog.ProcessingJobStatus = jobStatus;
		await SaveAsync();
	}

	/// <summary>
	/// Updates job notes
	/// </summary>
	/// <param name="processingJobID">current job id</param>
	/// <param name="notes">Notes</param>
	/// <returns>Awaitable task</returns>
	public async Task UpdateNotesAsync(Guid processingJobID, string notes)
	{
		var PcapFileProcessingLog = (
			from l in QueryAll<PcapFileProcessingLog>()
			where l.ProcessingJobID == processingJobID
			select l
		).Single();

		PcapFileProcessingLog.Notes = notes;

		await SaveAsync();
	}

	/// <summary>
	/// Inserts pcap meta data into db
	/// </summary>
	/// <param name="pcapMetaData">pcap meta data to insert</param>
	/// <returns>Event Meta data id.</returns>
	public async Task<long> InsertPcapMetaDataAsync(PcapMetaData pcapMetaData)
	{
		await CreateAsync(pcapMetaData);
		await SaveAsync();
		return pcapMetaData.PcapMetaDataID;
	}


	/// <summary>
	/// Inserts event meta data into db
	/// </summary>
	/// <param name="eventData">event data to insert</param>
	/// <returns>Event Meta data id.</returns>
	public async Task<long> InsertEventMetaDataAsync(EventMetaData eventData)
	{
		await CreateAsync(eventData);
		await SaveAsync();
		return eventData.EventMetaDataID;
	}
}
