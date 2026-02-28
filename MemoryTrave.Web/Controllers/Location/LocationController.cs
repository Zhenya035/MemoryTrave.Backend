using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Location;

[Route("locations")]
[Authorize]
public class LocationController(IWebHostEnvironment env) : BaseController(env)
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        throw new Exception();
    }

    [HttpGet("{locationId}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid locationId)
    {
        throw new Exception();
    }

    [HttpPost]
    public async Task<IActionResult> Add()
    {
        throw new Exception();
    }

    [HttpPut]
    public async Task<IActionResult> Update()
    {
        throw new Exception();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        throw new Exception();
    }
}