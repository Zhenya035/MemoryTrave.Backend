using FluentValidation;
using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Validators.Requests.Article.Access;

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
            .NotEmpty().WithMessage("Encrypted Keys is required")
            .NotNull().WithMessage("Encrypted Key cannot be null");
        
        RuleForEach(a => a.EncryptedKeys)
            .SetValidator(new AddAccessDtoValidator());
        
        RuleFor(a => a.LocationId)
            .NotEmpty().WithMessage("LocationId is required");
    }
}