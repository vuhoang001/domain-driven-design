using BuildingBlocks.Domain;

namespace Item.Domain.Item;

public class Item : Entity, IAggregateRoot
{
    public ItemId Id { get; private set; }
    public string ItemName { get; private set; }
    public string? ItemDesc { get; private set; }
    public decimal Price { get; private set; }

    private Item()
    {
        Id       = new ItemId(Guid.NewGuid());
        ItemName = "";
    }

    protected Item(ItemId id, string itemName, string? itemDesc, decimal price)
    {
        Id       = id;
        ItemName = itemName;
        ItemDesc = itemDesc;
        Price    = price;
    }

    public static Item Create(ItemId id, string itemName, string? itemDesc, decimal price)
    {
        var item = new Item(id, itemName, itemDesc, price);

        item.AddDomainEvent(new Events.CreateItemDomainEvent(id, itemName, itemDesc, price));

        return item;
    }
}