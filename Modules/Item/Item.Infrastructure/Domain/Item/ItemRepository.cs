using Item.Domain.Item;
using Microsoft.EntityFrameworkCore;

namespace Item.Infrastructure.Domain.Item;

public class ItemRepository(ItemContext itemContext) : IItemRepository
{
    public async Task AddAsync(global::Item.Domain.Item.Item item)
    {
        await itemContext.Items.AddAsync(item);
    }

    public async Task<global::Item.Domain.Item.Item?> GetByIdAsync(ItemId itemId)
    {
        return await itemContext.Items.FirstOrDefaultAsync(x => x.Id == itemId);
    }
}