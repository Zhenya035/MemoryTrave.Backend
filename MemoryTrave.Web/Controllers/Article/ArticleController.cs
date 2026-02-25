using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Article;

[Route("articles")]
[Authorize]
public class ArticleController : BaseController
{
    [HttpGet("{articleId}")]
    public async Task<IActionResult> Get(Guid articleId)
    {
        return Ok();
    }

    [HttpPost("private/add")]
    public async Task<IActionResult> AddPrivate()
    {
        return Ok();;
    }

    [HttpPost("public/add")]
    public async Task<IActionResult> AddPublic()
    {
        return Ok();;
    }

    [HttpPut("private/update")]
    public async Task<IActionResult> UpdatePrivate()
    {
        return Ok();
    }

    [HttpPut("public/update")]
    public async Task<IActionResult> UpdatePublic()
    {
        return Ok();
    }

    [HttpDelete("{articleId}/delete")]
    public async Task<IActionResult> Delete(Guid articleId)
    {
        return Ok();
    }
}