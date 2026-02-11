using CompanyName.MyMeetings.BuildingBlocks.Application.Outbox;

namespace MasterData.Infrastructure.Configuration;

public class Outbox : IOutbox
{
    public void Add(OutboxMessage message)
    {
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }
}