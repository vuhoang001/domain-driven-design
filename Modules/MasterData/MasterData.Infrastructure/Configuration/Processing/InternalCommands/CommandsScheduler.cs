using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.Serialization;
using Dapper;
using MasterData.Application.Configuration.Commands;
using MasterData.Application.Contracts;
using Newtonsoft.Json;

namespace Item.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory) : ICommandsScheduler
{
    public async Task EnqueueAsync(ICommand command)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sqlInsert = "INSERT INTO [dbo].[InternalCommands] ([Id], [Type], [Data], [OccurredOn]) VALUES " +
            "(@Id, @Type, @Data, @OccurredOn)";

        await connection.ExecuteAsync(sqlInsert, new
        {
            command.Id,
            Type = command.GetType().FullName,
            Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
            {
                ContractResolver = new AllPropertiesContractResolver()
            }),
            OccurredOn = DateTime.UtcNow
        });
    }

    public Task EnqueueAsync<T>(ICommand<T> command)
    {
        throw new NotImplementedException();
    }
}