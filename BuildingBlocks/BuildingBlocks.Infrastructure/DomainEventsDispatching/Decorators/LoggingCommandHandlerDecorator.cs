using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration.Commands;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching.Decorators;

public class LoggingCommandHandlerDecorator<T>(
    ILogger logger,
    IExecutionContextAccessor executionContextAccessor,
    ICommandHandler<T> decorated) : ICommandHandler<T> where T : ICommand
{
    public async Task Handle(T request, CancellationToken cancellationToken)
    {
        if (request is IRecurringCommand)
        {
            await decorated.Handle(request, cancellationToken);
        }

        using (
            LogContext.Push(
                new RequestLogEnricher(executionContextAccessor),
                new CommandLogEnricher(request)))
        {
            try
            {
                logger.Information(
                    "Executing command {Command}",
                    request.GetType().Name);

                await decorated.Handle(request, cancellationToken);

                logger.Information("Command {Command} processed successful", request.GetType().Name);
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Command {Command} processing failed", request.GetType().Name);
                throw;
            }
        }
    }

    private class CommandLogEnricher(ICommand command) : ILogEventEnricher
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