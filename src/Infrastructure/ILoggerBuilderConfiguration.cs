using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure;

public static class ILoggerBuilderConfiguration
{
    public static ILoggingBuilder AddLogger(this ILoggingBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        builder.ClearProviders().AddSerilog(logger);

        return builder;
    }
}
