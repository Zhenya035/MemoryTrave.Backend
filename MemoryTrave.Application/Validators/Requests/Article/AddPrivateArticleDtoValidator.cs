using FluentValidation;
using MemoryTrave.Application.Dto.Requests.Article;

namespace MemoryTrave.Application.Validators.Requests.Article;

public class AddPrivateArticleDtoValidator : AbstractValidator<AddPrivateArticleDto>
{
    public AddPrivateArticleDtoValidator()
    {
        RuleFor(a => a.EncryptedPreviewData)
            .NotEmpty().WithMessage("Encrypted Preview Data is required");
        
        RuleFor(a => a.EncryptedData)
            .NotEmpty().WithMessage("Encrypted Data is required");
        
        RuleFor(a => a.EncryptedKeys)
            .NotEmpty().WithMessage("Encrypted Keys is required");
        
        RuleFor(a => a.LocationId)
            .NotEmpty().WithMessage("LocationId is required");
    }
}