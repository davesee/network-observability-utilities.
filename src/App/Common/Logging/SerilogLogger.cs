using System;
using System.Reflection;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using Serilog.Templates;

namespace EFR.NetworkObservability.Common.Logging;

/// <summary>
/// Static Class containing configuration and logger for Serilog
/// </summary>
public static class SerilogLogger
{
	private static readonly string template = "[{@t:yyyy-MM-dd HH:mm:ss} {@l:u3}{#if SourceContext is not null} {SourceContext}{#end}] " +
		"{#if JobID is not null}[JOB_PROC_ID: {JobID}]{#end} " +
		"{@m}\n{@x}";

	private static readonly string index = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";

	/// <summary>
	/// Serilog Logger instance
	/// </summary>
	public static readonly ILogger instance;

	static SerilogLogger()
	{
		var elasticSearchURI = Environment.GetEnvironmentVariable(Constants.ELASTICSEARCH_URI);
		var loggerConfig = new LoggerConfiguration();

		loggerConfig
			.WriteTo.Console(new ExpressionTemplate(template))
			.Enrich.FromLogContext()
			.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
			.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Infrastructure", Serilog.Events.LogEventLevel.Warning);

		if (elasticSearchURI != null)
		{
			loggerConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchURI))
			{
				AutoRegisterTemplate = true,
				AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
				IndexFormat = index,
				CustomFormatter = new ElasticsearchJsonFormatter()
			});
		}

		instance = loggerConfig.CreateLogger();
	}
}
