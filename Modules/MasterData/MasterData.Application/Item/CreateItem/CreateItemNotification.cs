using BuildingBlocks.Application.Events;
using MasterData.Domain.Item.Events;

namespace MasterData.Application.Item.CreateItem;

public class CreateItemNotification(CreateItemDomainEvent domainEvent, Guid id)
    : DomainEventNotification<CreateItemDomainEvent>(domainEvent, id)
{
}