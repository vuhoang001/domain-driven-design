using BuildingBlocks.Application.Configuration.Commands;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemCommandHandler(
)
    : ICommandHandler<CreateItemCommand, string>
{
    public Task<string> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Item '{command.Name}' + Inventory created in same transaction");
    }
}