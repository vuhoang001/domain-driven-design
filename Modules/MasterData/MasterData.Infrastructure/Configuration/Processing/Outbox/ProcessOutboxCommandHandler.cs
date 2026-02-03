using BuildingBlocks.Application.Data;
using BuildingBlocks.Application.Events;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Dapper;
using MasterData.Application.Configuration.Commands;
using MediatR;
using Newtonsoft.Json;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Item.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler(
    IMediator mediator,
    ISqlConnectionFactory sqlConnectionFactory,
    IDomainNotificationMapper domainNotificationMapper) : ICommandHandler<ProcessOutboxCommand>
{
    public async Task Handle(ProcessOutboxCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = $"""
                            SELECT 
                                [OutboxMessage].[Id] AS [{nameof(OutboxMessageDto.Id)}], 
                                [OutboxMessage].[Type] AS [{nameof(OutboxMessageDto.Type)}], 
                                [OutboxMessage].[Data] AS [{nameof(OutboxMessageDto.Data)}] 
                            FROM [dbo].[OutboxMessages] AS [OutboxMessage] 
                            WHERE [OutboxMessage].[ProcessedDate] IS NULL 
                            ORDER BY [OutboxMessage].[OccurredOn]
                            """;

        var messages = await connection.QueryAsync<OutboxMessageDto>(sql);

        var messagesList = messages.ToList();

        const string sqlUpdateProcessedDate = """
                                              UPDATE [dbo].[OutboxMessages] 
                                              SET [ProcessedDate] = @Date 
                                              WHERE [Id] = @Id
                                              """;

        if (messagesList.Count > 0)
        {
            foreach (var message in messagesList)
            {
                var type   = domainNotificationMapper.GetType(message.Type);
                var @event = JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification;

                using (LogContext.Push(new OutboxMessageContextEnricher(@event!)))
                {
                    await mediator.Publish(@event, cancellationToken);
                    await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                    {
                        Date = DateTime.UtcNow,
                        Id   = message.Id
                    });
                }
            }
        }
    }

    private class OutboxMessageContextEnricher(IDomainEventNotification domainEventNotification) : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty(
                                             "Context",
                                             new ScalarValue(
                                                 $"OutboxMesageId:{domainEventNotification.Id}, DomainEventType:{domainEventNotification.GetType().FullName}")));
        }
    }
}