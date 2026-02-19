using MemoryTrave.Application.Dto.Responses.User;

namespace MemoryTrave.Application.Services;

public interface IUserService
{
    public Task<PrivateKeyResponceDto> GetPrivateKey();
}