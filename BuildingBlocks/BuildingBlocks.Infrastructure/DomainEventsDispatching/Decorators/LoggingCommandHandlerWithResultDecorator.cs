using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration.Commands;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching.Decorators;

public class LoggingCommandHandlerWithResultDecorator<T, TResult>(
    ILogger logger,
    IExecutionContextAccessor executionContextAccessor,
    ICommandHandler<T, TResult> decorated)
    : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        if (command is IRecurringCommand)
        {
            return await decorated.Handle(command, cancellationToken);
        }

        using (
            LogContext.Push(
                new RequestLogEnricher(executionContextAccessor),
                new CommandLogEnricher(command)))
        {
            try
            {
                logger.Information(
                    "Executing command {@Command}",
                    command);

                var result = await decorated.Handle(command, cancellationToken);

                logger.Information("Command processed successful, result {Result}", result);

                return result;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Command processing failed");
                throw;
            }
        }
    }

    private class CommandLogEnricher(ICommand<TResult> command) : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(
                new LogEventProperty("Context", new ScalarValue($"Command:{command.Id.ToString()}")));
        }
    }

    private class RequestLogEnricher(IExecutionContextAccessor executionContextAccessor) : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (executionContextAccessor.IsAvailable)
            {
                logEvent.AddOrUpdateProperty(
                    new LogEventProperty("CorrelationId", new ScalarValue(executionContextAccessor.CorrelationId)));
            }
        }
    }
}