using MasterData.Application.Configuration.Commands;
using MediatR;

namespace MasterData.Application.Item.CreateItem;

public class CreateItemNotificationHandler(ICommandsScheduler commandsScheduler)
    : INotificationHandler<CreateItemNotification>
{
    public async Task Handle(CreateItemNotification notification, CancellationToken cancellationToken)
    {
        await commandsScheduler.EnqueueAsync(
            new SendMessageWhenCreateItemCommand(Guid.NewGuid(), notification.DomainEvent.ItemId));
    }
}