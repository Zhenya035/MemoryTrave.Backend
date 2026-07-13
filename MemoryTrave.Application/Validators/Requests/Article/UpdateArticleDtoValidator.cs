using FluentValidation;
using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Validators.Requests.Article.Access;
using MemoryTrave.Domain.Enums;

namespace MemoryTrave.Application.Validators.Requests.Article;

public class UpdateArticleDtoValidator : AbstractValidator<UpdateArticleDto>
{
    public UpdateArticleDtoValidator()
    {
        RuleFor(a => a.Visibility)
            .IsInEnum()
            .WithMessage("Visibility is invalid");
        
        RuleFor(a => a.EncryptedDescription)
            .NotEmpty()
            .When(a => a.Visibility == VisibilityEnum.Private)
            .WithMessage("Encrypted description is required for private article");
        
        RuleFor(a => a.EncryptedKeys)
            .NotEmpty()
            .When(a => a.Visibility == VisibilityEnum.Private)
            .WithMessage("Encrypted keys is required for private article");
        
        RuleForEach(a => a.EncryptedKeys)
            .SetValidator(new AddAccessDtoValidator())
            .When(a => a.Visibility == VisibilityEnum.Private && a.EncryptedKeys != null);
        
        RuleFor(a => a.Description)
            .NotEmpty()
            .When(a => a.Visibility == VisibilityEnum.Public)
            .WithMessage("Description is required for public article");
    }
}