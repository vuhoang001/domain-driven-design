using MasterData.Domain.Item;

namespace MasterData.IntegrationEvent;

public class CreateItemIntegrationEvent(
    Guid id,
    DateTime occurredOn,
    ItemId itemId,
    string itemName,
    string? itemDesc,
    decimal price) : BuildingBlocks.Infrastructure.EventBus.IntegrationEvent(id, occurredOn)
{
    public ItemId ItemId { get; set; } = itemId;
    public string ItemName { get; set; } = itemName;
    public string? ItemDesc { get; set; } = itemDesc;
    public decimal Price { get; set; } = price;
}