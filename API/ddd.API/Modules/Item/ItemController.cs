using Item.Application.Configuration.Commands;
using Microsoft.AspNetCore.Mvc;

namespace ddd.API.Modules.Item;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemModule _itemModule;
    private readonly ILogger<ItemController> _logger;

    public ItemController(IItemModule itemModule, ILogger<ItemController> logger)
    {
        _itemModule = itemModule;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Getting all items");
        // TODO: Implement GetAllItemsQuery
        return Ok(new { message = "Item module is working!" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Getting item with id: {Id}", id);
        // TODO: Implement GetItemByIdQuery
        return Ok(new { id, message = "Item found!" });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemRequest request)
    {
        _logger.LogInformation("Creating new item: {@Request}", request);
        // TODO: Implement CreateItemCommand
        return CreatedAtAction(nameof(GetById), new { id = Guid.NewGuid() }, new { message = "Item created!" });
    }
}

public record CreateItemRequest(string Name, string Description);
