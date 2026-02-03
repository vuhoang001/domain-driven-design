using BuildingBlocks.Application.Data;
using Dapper;
using MasterData.Application.Configuration.Commands;
using Newtonsoft.Json;
using Polly;

namespace Item.Infrastructure.Configuration.Processing.InternalCommands;

public class ProcessInternalCommandsCommandHandler(ISqlConnectionFactory sqlConnectionFactory)
    : ICommandHandler<ProcessInternalCommandsCommand>
{
    public async Task Handle(ProcessInternalCommandsCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = $"""
                            SELECT 
                                [Command].[Id] AS [{nameof(InternalCommandDto.Id)}], 
                                [Command].[Type] AS [{nameof(InternalCommandDto.Type)}], 
                                [Command].[Data] AS [{nameof(InternalCommandDto.Data)}] 
                            FROM [dbo].[InternalCommands] AS [Command] 
                            WHERE [Command].[ProcessedDate] IS NULL 
                            ORDER BY [Command].[EnqueueDate]
                            """;

        var commands = await connection.QueryAsync<InternalCommandDto>(sql);

        var internalCommandsList = commands.AsList();

        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync([
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3)
            ]);

        foreach (var internalCommand in internalCommandsList)
        {
            var result = await policy.ExecuteAndCaptureAsync(() => ProcessCommand(internalCommand));
            if (result.Outcome == OutcomeType.Failure)
            {
                await connection.ExecuteScalarAsync(
                    """
                    UPDATE [dbo].[InternalCommands] 
                    SET ProcessedDate = @NowDate, Error = @Error
                    WHERE [Id] = @Id
                    """,
                    new
                    {
                        NowDate = DateTime.UtcNow,
                        Error   = result.FinalException.ToString(),
                        internalCommand.Id
                    });
            }
            else if (result.Outcome == OutcomeType.Successful)
            {
                await connection.ExecuteScalarAsync(
                    """
                    UPDATE [dbo].[InternalCommands] 
                    SET ProcessedDate = @NowDate
                    WHERE [Id] = @Id
                    """,
                    new
                    {
                        NowDate = DateTime.UtcNow,
                        internalCommand.Id
                    });
            }
        }
    }

    private async Task ProcessCommand(InternalCommandDto internalCommand)
    {
        var type = Assemblies.Application.GetType(internalCommand.Type);
        if (type is null) return;

        dynamic? commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type);

        await CommandsExecutor.Execute(commandToProcess);
    }

    private class InternalCommandDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public string Data { get; set; } = null!;
    }
}