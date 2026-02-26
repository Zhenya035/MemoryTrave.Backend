using FluentValidation;
using MemoryTrave.Application.Dto.Requests;

namespace MemoryTrave.Application.Validators.Requests;

public class ListIdDtoValidator : AbstractValidator<ListIdDto>
{
    public ListIdDtoValidator()
    {
        RuleFor(r => r.Ids)
            .NotNull().WithMessage("Ids list cannot be null.")
            .NotEmpty().WithMessage("Ids list cannot be empty.");
    }
}