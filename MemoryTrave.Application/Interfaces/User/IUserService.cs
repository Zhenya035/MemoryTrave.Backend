using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;

namespace MemoryTrave.Application.Interfaces.User;

public interface IUserService
{
    //for authorization
    public Task<AuthorizationResponseDto> Authorization(AuthorizationDto authUser);
    public Task<PrivateKeyResponseDto> GetPrivateKey(Guid userId);
    
    //for registration
    public Task<AuthorizationResponseDto> Registration(RegistrationDto regUser);
    public Task AddKeys(AddKeysDto keys, Guid userId);
    
    //for profile
    public Task<GetProfileDto> GetProfile(Guid userId);
    public Task<List<GetUserDto>> GetBlockUsers(Guid userId);
    public Task DeleteProfile(Guid userId);
}