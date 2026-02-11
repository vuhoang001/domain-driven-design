namespace MasterData.Domain.Entities.Item;

public interface ItemRepository
{
    Task Add(Item item);
}