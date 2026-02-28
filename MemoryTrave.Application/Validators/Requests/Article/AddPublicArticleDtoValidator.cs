using FluentValidation;
using MemoryTrave.Application.Dto.Requests.Article;

namespace MemoryTrave.Application.Validators.Requests.Article;

public class AddPublicArticleDtoValidator : AbstractValidator<AddPublicArticleDto>
{
    public AddPublicArticleDtoValidator()
    {
        RuleFor(a => a.Description)
            .NotEmpty().WithMessage("Description is required");
        
        RuleFor(a => a.PhotosUrls)
            .NotEmpty().WithMessage("PhotosUrls is required");
        
        RuleFor(a => a.LocationId)
            .NotEmpty().WithMessage("LocationId is required");
    }
}