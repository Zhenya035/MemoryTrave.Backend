using MemoryTrave.Application.Dto.Requests.Location;
using MemoryTrave.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers;

[Route("locations")]
[Authorize]
public class LocationController(
    ILocationService service,
    IWebHostEnvironment env) : BaseController(env)
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromBody] GetLocationRequestDto dto)
    {
        var result = await service.GetAll(dto);
        return HandleResult(result);
    }

    [HttpGet("{locationId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid locationId)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.GetById(locationId, userId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddAndUpdateLocationDto dto)
    {
        var result = await service.Add(dto);
        return HandleResult(result);
    }

    [HttpPut("{locationId:guid}")]
    public async Task<IActionResult> Update(Guid locationId, [FromBody] AddAndUpdateLocationDto dto)
    {
        var result = await service.Update(dto, locationId);
        return HandleResult(result);
    }

    [HttpDelete("{locationId:guid}")]
    public async Task<IActionResult> Delete(Guid locationId)
    {
        var result = await service.Delete(locationId);
        return HandleResult(result);
    }
}