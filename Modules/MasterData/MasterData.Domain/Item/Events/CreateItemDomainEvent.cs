using BuildingBlocks.Domain;

namespace MasterData.Domain.Item.Events;

public class CreateItemDomainEvent(ItemId itemId, string itemName, string? itemDesc, decimal price)
    : DomainEventBase
{
    public ItemId ItemId { get; } = itemId;
    public string ItemName { get; } = itemName;
    public string? ItemDesc { get; } = itemDesc;
    public decimal Price { get; } = price;
}