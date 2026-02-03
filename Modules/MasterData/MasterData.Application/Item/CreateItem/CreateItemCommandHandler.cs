using MasterData.Application.Configuration.Commands;
using MasterData.Domain.Item;

namespace MasterData.Application.Item.CreateItem;

public class CreateItemCommandHandler(IItemRepository itemRepository) : ICommandHandler<CreateItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = global::MasterData.Domain.Item.Item.Create(new ItemId(Guid.NewGuid()), request.ItemName,
                                                              request.ItemDesc,
                                                              request.Price);

        await itemRepository.AddAsync(item);
        return item.Id.Value;
    }
}