using FluentValidation;
using MemoryTrave.Application.Dto;

namespace MemoryTrave.Application.Validators.Requests;

public class ListIdDtoValidator : AbstractValidator<ListIdDto>
{
    public ListIdDtoValidator()
    {
        RuleFor(r => r.Ids)
            .NotNull().WithMessage("Ids list cannot be null.")
            .NotEmpty().WithMessage("Ids list cannot be empty.").Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("Ids list must not contain duplicates.");
        
        RuleForEach(r => r.Ids)
            .NotEmpty().WithMessage("Each Id must not be empty.");
    }
}