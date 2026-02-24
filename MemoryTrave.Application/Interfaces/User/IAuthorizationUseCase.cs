using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;

namespace MemoryTrave.Application.Interfaces.User;

public interface IAuthorizationUseCase
{
    public Task<AuthorizationResponseDto> Authorization(AuthorizationDto authUser);
    public Task<PrivateKeyResponceDto> GetPrivateKey();
}