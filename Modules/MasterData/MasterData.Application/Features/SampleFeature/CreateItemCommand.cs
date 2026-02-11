
using BuildingBlocks.Application.Contracts.Commands;

namespace MasterData.Application.Features.SampleFeature;

public class CreateItemCommand(string name) : CommandBase<string>
{
    public string Name { get; } = name;
}