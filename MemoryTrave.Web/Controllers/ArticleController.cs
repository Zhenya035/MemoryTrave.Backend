using MemoryTrave.Application.Dto.Photo;
using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Dto.Requests.Article.Access;
using MemoryTrave.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers;

[Route("articles")]
[Authorize]
public class ArticleController(IArticleService service, IWebHostEnvironment env) : BaseController(env)
{
    [HttpGet("{articleId}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid articleId)
    {
        Guid userId;
        try
        {
            userId = GetCurrentUserId();
        }
        catch (UnauthorizedAccessException e)
        {
            userId = Guid.Empty;
        }
        
        var result = await service.GetByIdWithIncludes(articleId, userId);
        return HandleResult(result);
    }

    [HttpGet("private")]
    public async Task<IActionResult> GetPrivateArticles()
    {
        var userId = GetCurrentUserId();
        
        var result = await service.GetPrivate(userId);
        return HandleResult(result);
    }

    [HttpGet("{articleId:guid}/author")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAuthor(Guid articleId)
    {
        var result = await service.GetAuthor(articleId);
        return HandleResult(result);
    }

    [HttpPost("access/{userId:guid}")]
    public async Task<IActionResult> AddAccess(Guid userId, [FromBody] List<AddAccessForFriendDto> dto)
    {
        var result = await service.AddAccess(dto, userId);
        return HandleResult(result);
    }

    [HttpPost("private/{locationId:guid}/create")]
    public async Task<IActionResult> AddPrivate(Guid locationId)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.AddPrivate(locationId, userId);
        return HandleResult(result);
    }

    [HttpPost("public")]
    public async Task<IActionResult> AddPublic([FromBody] AddPublicArticleDto dto)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.AddPublic(dto, userId);
        return HandleResult(result);
    }
    
    [HttpPost("private/{articleId:guid}/data")]
    public async Task<IActionResult> AddPhotoToPrivate(Guid articleId, [FromBody] AddPrivateArticleDto dto)
    {
        var result = await service.AddDataToPrivate(dto, articleId);
        return HandleResult(result);
    }
    
    [HttpPut("{articleId:guid}")]
    public async Task<IActionResult> Update(Guid articleId, [FromBody] UpdateArticleDto dto)
    {
        var result = await service.Update(dto, articleId);
        return HandleResult(result);
    }

    [HttpDelete("{articleId:guid}")]
    public async Task<IActionResult> Delete(Guid articleId)
    {
        var result = await service.Delete(articleId);
        return HandleResult(result);
    }
}