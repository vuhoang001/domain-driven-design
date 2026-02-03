namespace MasterData.Domain.Item;

public interface IItemRepository
{
    Task       AddAsync(Item item);
    Task<Item?> GetByIdAsync(ItemId itemId);
}