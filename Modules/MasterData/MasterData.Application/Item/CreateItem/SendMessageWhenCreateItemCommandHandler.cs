using MasterData.Application.Configuration.Commands;

namespace MasterData.Application.Item.CreateItem;

public class SendMessageWhenCreateItemCommandHandler
    : ICommandHandler<SendMessageWhenCreateItemCommand>
{
    public Task Handle(SendMessageWhenCreateItemCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Send message successfully");
        return Task.CompletedTask;
    }
}