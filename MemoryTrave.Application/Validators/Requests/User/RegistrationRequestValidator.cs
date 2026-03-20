using FluentValidation;
using MemoryTrave.Application.Dto.Requests.User;

namespace MemoryTrave.Application.Validators.Requests.User;

public class RegistrationRequestValidator : AbstractValidator<RegistrationDto>
{
    public RegistrationRequestValidator()
    {
        RuleFor(r =>r.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid Email.");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters.")
            .MaximumLength(128).WithMessage("Password must have no more than 128 characters.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
            .WithMessage("Password must consist of uppercase, lowercase letters and numbers.");
        
        RuleFor(r => r.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must have at least 3 characters.")
            .MaximumLength(50).WithMessage("Username must have no more than 50 characters.");
    }
}