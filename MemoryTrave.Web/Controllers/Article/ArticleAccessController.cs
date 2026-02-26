using FluentValidation;
using MemoryTrave.Application.Dto.Requests;
using MemoryTrave.Application.Dto.Requests.Article.Access;
using MemoryTrave.Application.Interfaces.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Article;

[Route("articles/access")]
[Authorize]
public class ArticleAccessController(
    IArticleAccessService service,
    IValidator<AddListAccessDto> addListValidator,
    IValidator<AddAccessDto> addAccessValidator,
    IValidator<ListIdDto> listIdValidator) : BaseController
{
    [HttpPost("add/list")]
    public async Task<IActionResult> AddList(AddListAccessDto dto)
    {
        var validResult = await addListValidator.ValidateAsync(dto);
        if(!validResult.IsValid)
            return ValidFailed(validResult);

        await service.AddList(dto);
        return Success();
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddAccessDto dto)
    {
        var validResult = await addAccessValidator.ValidateAsync(dto);
        if(!validResult.IsValid)
            return ValidFailed(validResult);
        
        await service.Add(dto);
        return Success();
    }

    [HttpDelete("list")]
    public async Task<IActionResult> DeleteList([FromBody] ListIdDto dto)
    {
        var validResult = await listIdValidator.ValidateAsync(dto);
        if(!validResult.IsValid)
            return ValidFailed(validResult);
        
        await service.DeleteList(dto);
        return Success();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.Delete(id);
        return Success();
    }
}