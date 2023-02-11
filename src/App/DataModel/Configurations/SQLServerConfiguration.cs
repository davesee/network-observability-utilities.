using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Diagnostics.CodeAnalysis;
using EFR.NetworkObservability.Common;

namespace EFR.NetworkObservability.DataModel.Configurations;

[ExcludeFromCodeCoverage]
internal class SQLServerConfiguration : DbConfiguration
{
	public SQLServerConfiguration()
	{
		var retryCount = Utils.GetEnvVarOrDefault("SQL_SERVER_RETRY_COUNT", 3);
		var retryTime = Utils.GetEnvVarOrDefault("SQL_SERVER_RETRY_TIME", 30);

		SetExecutionStrategy(
			"System.Data.SqlClient",
			() => new SqlAzureExecutionStrategy(retryCount, TimeSpan.FromSeconds(retryTime)));
	}
}
