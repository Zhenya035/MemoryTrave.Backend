using FluentValidation;
using MemoryTrave.Application.Dto.Requests.User;

namespace MemoryTrave.Application.Validators.Requests.User;

public class AddKeysRequestValidator : AbstractValidator<AddKeysDto>
{
    public AddKeysRequestValidator()
    {
        RuleFor(r => r.PublicKey)
            .NotEmpty().WithMessage("Public Key is required.");
        
        RuleFor(r => r.EncryptedPrivateKey)
            .NotEmpty().WithMessage("Encrypted Private Key is required.");
    }
}