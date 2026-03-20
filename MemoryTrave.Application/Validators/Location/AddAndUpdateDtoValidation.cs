using FluentValidation;
using MemoryTrave.Application.Dto.Requests.Location;

namespace MemoryTrave.Application.Validators.Location;

public class AddAndUpdateDtoValidation : AbstractValidator<AddAndUpdateLocationDto>
{
    public AddAndUpdateDtoValidation()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Name is required");
        
        RuleFor(dto => dto.Latitude)
            .NotNull().WithMessage("Latitude is required")
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");
        
        RuleFor(dto => dto.Longitude)
            .NotNull().WithMessage("Longitude is required")
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180 degrees.");
    }
}