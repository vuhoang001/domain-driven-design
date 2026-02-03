using Quartz.Logging;
using Serilog;

namespace Item.Infrastructure.Configuration.Quartz;

public class SerilogLogProvider(ILogger logger) : ILogProvider
{
    public Logger GetLogger(string name)
    {
        return (level, func, exception, parameters) =>
        {
            if (func == null)
            {
                return true;
            }

            if (level == LogLevel.Debug || level == LogLevel.Trace)
            {
                logger.Debug(exception, func(), parameters);
            }

            if (level == LogLevel.Info)
            {
                logger.Information(exception, func(), parameters);
            }

            if (level == LogLevel.Warn)
            {
                logger.Warning(exception, func(), parameters);
            }

            if (level == LogLevel.Error)
            {
                logger.Error(exception, func(), parameters);
            }

            if (level == LogLevel.Fatal)
            {
                logger.Fatal(exception, func(), parameters);
            }

            return true;
        };
    }

    public IDisposable OpenNestedContext(string message)
    {
        throw new NotImplementedException();
    }

    public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
    {
        throw new NotImplementedException();
    }
}