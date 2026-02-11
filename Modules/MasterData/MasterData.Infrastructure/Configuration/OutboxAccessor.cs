using BuildingBlocks.Application.Outbox;

namespace MasterData.Infrastructure.Configuration;

public class OutboxAccessor(MasterDataContext meetingsContext) : IOutbox
{
    public void Add(OutboxMessage message)
    {
        meetingsContext.OutboxMessages.Add(message);
    }

    public Task Save()
    {
        return
            Task.CompletedTask; // Save is done automatically using EF Core Change Tracking mechanism during SaveChanges.
    }
}