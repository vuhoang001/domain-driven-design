using MasterData.Application.Contracts;
using Newtonsoft.Json;

namespace MasterData.Application.Item.CreateItem;

[method: JsonConstructor]
public class CreateItemCommand(string itemName, string? itemDesc, decimal price) : CommandBase<Guid>
{
    internal string ItemName { get; } = itemName;
    internal string? ItemDesc { get; } = itemDesc;
    internal decimal Price { get; } = price;
}