using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;

namespace MemoryTrave.Application.Services.User;

public interface IRegistrationUseCase
{
    public Task<AuthorizationResponseDto> Registration(RegistrationDto regUser);
}