using FluentValidation;
using MemoryTrave.Application.Dto.Requests.Article.Access;

namespace MemoryTrave.Application.Validators.Requests.Article.Access;

public class AddAccessForFriendValidator : AbstractValidator<AddAccessForFriendDto>
{
    public AddAccessForFriendValidator()
    {
        RuleFor(r => r.ArticleId)
            .NotEmpty().WithMessage("ArticleId is required");

        RuleFor(r => r.EncryptedKey)
            .NotEmpty().WithMessage("EncryptedKey is required");
    }
}