using MasterData.Domain.Entities.Item;

namespace MasterData.Infrastructure.Entities.Item;

public class ItemRepository(MasterDataContext dataContext) : IItemRepository
{
    public void Add(Domain.Entities.Item.Item item)
    {
        dataContext.Items.Add(item);
    }
}