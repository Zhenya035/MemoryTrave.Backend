using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Domain.Exceptions;
using MemoryTrave.Domain.Interfaces;

namespace MemoryTrave.Application.Services.User;

public class UserService(IUserRepository repository, ICurrentUserProvider user) : IUserService
{
    public async Task<PrivateKeyResponceDto> GetPrivateKey()
    {
        var userId = user.GetUserId();

        if (!await repository.UserExistsById(userId))
            throw new NotFoundException("User");
        
        var encryptedPrivateKey = await repository.GetKeyById(userId);
        
        if(encryptedPrivateKey == null)
            throw new NotFoundException("PrivateKey");

        var response = new PrivateKeyResponceDto()
        {
            EncryptedPrivateKey = encryptedPrivateKey
        };
        return response;
    }

    public async Task AddKeys(AddKeysDto keys)
    {
        var userId = user.GetUserId();
        
        if (!await repository.UserExistsById(userId))
            throw new NotFoundException("User");
        
        await repository.AddKey(userId, keys.PublicKey, keys.EncryptedPrivateKey);
    }
}