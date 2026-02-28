using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Dto.Responses.Article;
using MemoryTrave.Application.Interfaces.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Article;

[Route("articles")]
[Authorize]
public class ArticleController(IArticleService service, IWebHostEnvironment env) : BaseController(env)
{
    [HttpGet("{articleId}")]
    public async Task<IActionResult> Get(Guid articleId)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.GetByIdWithIncludes(articleId, userId);
        return HandleResult(result);
    }

    [HttpPost("private/add")]
    public async Task<IActionResult> AddPrivate([FromBody] AddPrivateArticleDto dto)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.AddPrivate(dto, userId);
        return HandleResult(result);
    }

    [HttpPost("public/add")]
    public async Task<IActionResult> AddPublic([FromBody] AddPublicArticleDto dto)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.AddPublic(dto, userId);
        return HandleResult(result);
    }
    
    [HttpPut("{articleId:guid}")]
    public async Task<IActionResult> Update(Guid articleId, [FromBody] UpdateArticleDto dto)
    {
        var result = await service.Update(dto, articleId);
        return HandleResult(result);
    }

    [HttpDelete("{articleId}/delete")]
    public async Task<IActionResult> Delete(Guid articleId)
    {
        var result = await service.Delete(articleId);
        return HandleResult(result);
    }
}