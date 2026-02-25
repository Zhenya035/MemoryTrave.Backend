using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Location;

[Route("locations")]
[Authorize]
public class LocationController : BaseController
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }

    [HttpGet("{locationId}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid locationId)
    {
        return Ok();
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add()
    {
        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update()
    {
        return Ok();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        return Ok();
    }
}