using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Article;

[Route("articles/access")]
[Authorize]
public class ArticleAccessController : BaseController
{
    [HttpPost("add/list")]
    public async Task<IActionResult> AddList()
    {
        return Ok();
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add()
    {
        return Ok();
    }
}