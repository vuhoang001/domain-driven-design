using MasterData.Domain.Item;
using Microsoft.EntityFrameworkCore;

namespace Item.Infrastructure.Domain.Item;

public class ItemRepository(MasterDataContext masterDataContext) : IItemRepository
{
    public async Task AddAsync(global::MasterData.Domain.Item.Item item)
    {
        await masterDataContext.Items.AddAsync(item);
    }

    public async Task<global::MasterData.Domain.Item.Item?> GetByIdAsync(ItemId itemId)
    {
        return await masterDataContext.Items.FirstOrDefaultAsync(x => x.Id == itemId);
    }
}