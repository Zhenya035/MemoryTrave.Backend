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

    [HttpPost]
    public async Task<IActionResult> Add()
    {
        return Success();
    }

    [HttpPut]
    public async Task<IActionResult> Update()
    {
        return Success();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        return Success();
    }
}