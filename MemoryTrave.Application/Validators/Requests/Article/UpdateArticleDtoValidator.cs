using FluentValidation;
using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Domain.Enums;

namespace MemoryTrave.Application.Validators.Requests.Article;

public class UpdateArticleDtoValidator : AbstractValidator<UpdateArticleDto>
{
    public UpdateArticleDtoValidator()
    {
        RuleFor(a => a.Visibility)
            .IsInEnum()
            .WithMessage("Visibility is invalid");
        
        RuleFor(a => a.EncryptedPreviewData)
            .NotEmpty()
            .When(a => a.Visibility == VisibilityEnum.Private)
            .WithMessage("Preview encrypted data is required for private article");
        
        RuleFor(a => a.EncryptedData)
            .NotEmpty()
            .When(a => a.Visibility == VisibilityEnum.Private)
            .WithMessage("Encrypted data is required for private article");
        
        RuleFor(a => a.EncryptedKeys)
            .NotEmpty()
            .When(a => a.Visibility == VisibilityEnum.Private)
            .WithMessage("Encrypted keys is required for private article");
        
        RuleFor(a => a.Description)
            .NotEmpty()
            .When(a => a.Visibility == VisibilityEnum.Public)
            .WithMessage("Description is required for public article");
        
        RuleFor(a => a.PhotosUrls)
            .NotEmpty()
            .When(a => a.Visibility == VisibilityEnum.Public)
            .WithMessage("Photos urls is required for public article");
    }
}