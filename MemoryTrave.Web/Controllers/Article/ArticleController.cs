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
        return Success();
    }

    [HttpPost("private/add")]
    public async Task<IActionResult> AddPrivate()
    {
        return Success();;
    }

    [HttpPost("public/add")]
    public async Task<IActionResult> AddPublic()
    {
        return Success();;
    }

    [HttpPut("private/update")]
    public async Task<IActionResult> UpdatePrivate()
    {
        return Success();
    }

    [HttpPut("public/update")]
    public async Task<IActionResult> UpdatePublic()
    {
        return Success();
    }

    [HttpDelete("{articleId}/delete")]
    public async Task<IActionResult> Delete(Guid articleId)
    {
        return Success();
    }
}