using FluentValidation;
using MemoryTrave.Application.Dto.Requests.Article.Access;

namespace MemoryTrave.Application.Validators.Requests.Article.Access;

public class AddListAccessDtoValidator : AbstractValidator<AddListAccessDto>
{
    public AddListAccessDtoValidator()
    {
        RuleFor(r => r.Items)
            .NotNull().WithMessage("Access requests list cannot be null.")
            .NotEmpty().WithMessage("Access requests list cannot be empty.");

        RuleForEach(r => r.Items).SetValidator(new AddAccessDtoValidator());
        
        RuleFor(r => r.Items)
            .Must(items => items
                .GroupBy(i => i.UserId)
                .All(g => g.Count() == 1))
            .WithMessage("The list of access requests must not contain duplicates.");
    }
}