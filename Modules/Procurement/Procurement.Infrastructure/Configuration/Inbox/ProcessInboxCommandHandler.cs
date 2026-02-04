using BuildingBlocks.Application.Data;
using Dapper;
using MediatR;
using Newtonsoft.Json;

namespace Procurement.Infrastructure.Configuration.Inbox;

public class ProcessInboxCommandHandler(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
    : IRequestHandler<ProcessInboxCommand>
{
    public async Task Handle(ProcessInboxCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        const string sql = $"""
                            SELECT [InboxMessage].[Id] AS [{nameof(InboxMessageDto.Id)}], 
                                   [InboxMessage].[Type] AS [{nameof(InboxMessageDto.Type)}], 
                                   [InboxMessage].[Data] AS [{nameof(InboxMessageDto.Data)}] 
                            FROM [dbo].[InboxMessages] AS [InboxMessage] 
                            WHERE [InboxMessage].[ProcessedDate] IS NULL 
                            ORDER BY [InboxMessage].[OccurredOn]
                            """;

        var messages = await connection.QueryAsync<InboxMessageDto>(sql);

        const string sqlUpdateProcessedDate = """
                                              UPDATE [meetings].[InboxMessages] 
                                              SET [ProcessedDate] = @Date
                                              WHERE [Id] = @Id
                                              """;

        foreach (var message in messages)
        {
            var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => message.Type.Contains(assembly.GetName().Name!));
            if (messageAssembly is null) continue;

            var type = messageAssembly.GetType(message.Type)!;
            var  req  = JsonConvert.DeserializeObject(message.Data, type);

            try
            {
                await mediator.Publish((INotification)req!, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            await connection.ExecuteScalarAsync(sqlUpdateProcessedDate, new
            {
                Date = DateTime.UtcNow,
                message.Id
            });
        }
    }
}