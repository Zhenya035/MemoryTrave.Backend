using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces;

public interface IValidationService
{
    public Task<Result> Validate<T>(T dto);
}