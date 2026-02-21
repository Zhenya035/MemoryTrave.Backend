using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;

namespace MemoryTrave.Application.Interfaces.User;

public interface IUserService
{
    public Task<PrivateKeyResponceDto> GetPrivateKey();
    public Task AddKeys(AddKeysDto keys);
}