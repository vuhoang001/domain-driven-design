using MasterData.Application.Configuration.Command;
using MasterData.Application.Features.SampleFeature;
using Microsoft.AspNetCore.Mvc;

namespace ddd.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IMasterDataModule masterDataModule) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateItemCommand command)
    {
        var result = await masterDataModule.ExecuteCommandAsync(command);
        return Ok(new { message = result });
    }
}