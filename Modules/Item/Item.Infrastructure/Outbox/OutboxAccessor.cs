using BuildingBlocks.Application.Outbox;

namespace Item.Infrastructure.Outbox;

public class OutboxAccessor : IOutbox
{
    public void Add(OutboxMessage message)
    {
        throw new NotImplementedException();
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }
}