using FluentValidation;
using MemoryTrave.Application.Dto.Requests.Article.Access;

namespace MemoryTrave.Application.Validators.Requests.Article.Access;

public class AddAccessDtoValidator : AbstractValidator<AddAccessDto>
{
    public AddAccessDtoValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty().WithMessage("UserId is required");
        
        RuleFor(r => r.ArticleId)
            .NotEmpty().WithMessage("ArticleId is required");
        
        RuleFor(r => r.EncryptedKey)
            .NotEmpty().WithMessage("EncryptedKey is required")
            .Length(152).WithMessage("EncryptedKey must be exactly 152 characters long");
    }
}