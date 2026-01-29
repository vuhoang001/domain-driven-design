using Item.Application.Configuration.Commands;
using Item.Domain.Item;

namespace Item.Application.Item;

public class CreateItemCommandHandler(IItemRepository itemRepository) : ICommandHandler<CreateItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = Domain.Item.Item.Create(new ItemId(Guid.NewGuid()), request.ItemName, request.ItemDesc,
                                           request.Price);

        await itemRepository.AddAsync(item);
        return item.Id.Value;
    }
}