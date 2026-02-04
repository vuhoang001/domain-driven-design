using Autofac;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure.EventBus;
using BuildingBlocks.Infrastructure.Serialization;
using Dapper;
using Newtonsoft.Json;

namespace Procurement.Infrastructure.Configuration.EventBus;

public class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T> where T : IntegrationEvent
{
    public async Task Handle(T @event)
    {
        using (var scope = ProcurementCompositionRoot.BeginLifetimeScope())
        {
            using (var connection = scope.Resolve<ISqlConnectionFactory>().GetOpenConnection())
            {
                var type = @event.GetType().FullName;
                var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                });


                var sql = "INSERT INTO [meetings].[InboxMessages] (Id, OccurredOn, Type, Data) " +
                    "VALUES (@Id, @OccurredOn, @Type, @Data)";

                await connection.ExecuteScalarAsync(sql, new
                {
                    @event.Id,
                    @event.OccurredOn,
                    type,
                    data
                });
            }
        }
    }
}