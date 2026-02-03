using BuildingBlocks.Application.Outbox;

namespace Item.Infrastructure.Outbox;

public class OutboxAccessor(MasterDataContext masterDataContext) : IOutbox
{
    
    public void Add(OutboxMessage message)
    {
        masterDataContext.OutboxMessages.Add(message);
    }

    public Task Save()
    {
        // Save is done automatically using EF Core Change Tracking mechanism during SaveChanges.
        return Task.CompletedTask;
    }
}