using MasterData.Application.Configuration.Commands;
using MasterData.Application.Item.CreateItem;
using Microsoft.AspNetCore.Mvc;

namespace ddd.API.Modules.Item;

[ApiController]
[Route("api/[controller]")]
public class ItemController(IMasterDataModule itemModule, ILogger<ItemController> logger) : ControllerBase
{
    private readonly IMasterDataModule _itemModule = itemModule;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        logger.LogInformation("Getting all items");
        return Ok(new { message = "Item module is working!" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        logger.LogInformation("Getting item with id: {Id}", id);
        // TODO: Implement GetItemByIdQuery
        return Ok(new { id, message = "Item found!" });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateItemDto request)
    {
        await itemModule.ExecuteCommandAsync(new CreateItemCommand(request.ItemName, request.ItemDesc, request.Price));
        return CreatedAtAction(nameof(GetById), new { id = Guid.NewGuid() }, new { message = "Item created!" });
    }
}
