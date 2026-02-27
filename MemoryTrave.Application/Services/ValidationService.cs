using FluentValidation;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryTrave.Application.Services;

public class ValidationService(IServiceProvider serviceProvider) : IValidationService
{
    public async Task<Result> Validate<T>(T dto)
    {
        var validator = serviceProvider.GetService<IValidator<T>>();
        
        if (validator == null) 
            return Result.Success();
        
        var validationResult = await validator.ValidateAsync(dto);

        if (validationResult.IsValid) return Result.Success();
        
        var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
        return Result.Failure(errors, ErrorCode.InvalidInput);

    }
}