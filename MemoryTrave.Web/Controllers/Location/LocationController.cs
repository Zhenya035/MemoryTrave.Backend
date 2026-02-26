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
        return Success();
    }

    [HttpGet("{locationId}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid locationId)
    {
        return Success();
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add()
    {
        return Success();
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update()
    {
        return Success();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        return Success();
    }
}