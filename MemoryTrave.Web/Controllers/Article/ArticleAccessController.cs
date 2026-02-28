using MemoryTrave.Application.Dto.Requests;
using MemoryTrave.Application.Dto.Requests.Article.Access;
using MemoryTrave.Application.Interfaces.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Article;

[Route("articles/access")]
[Authorize]
public class ArticleAccessController(IArticleAccessService service, IWebHostEnvironment env) : BaseController(env)
{
    [HttpPost("add/list")]
    public async Task<IActionResult> AddList(AddListAccessDto dto)
    {
        var result = await service.AddList(dto);
        return HandleResult(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddAccessDto dto)
    {
        var result = await service.Add(dto);
        return HandleResult(result);
    }

    [HttpDelete("list")]
    public async Task<IActionResult> DeleteList([FromBody] ListIdDto dto)
    {
        var result = await service.DeleteList(dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await service.Delete(id);
        return HandleResult(result);
    }
}