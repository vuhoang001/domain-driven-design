using MasterData.Application.Configuration.Commands;
using MasterData.Domain.Item;

namespace MasterData.Application.Item.CreateItem;

public  class SendMessageWhenCreateItemCommand(Guid id, ItemId itemId) : InternalCommandBase(id)
{
}