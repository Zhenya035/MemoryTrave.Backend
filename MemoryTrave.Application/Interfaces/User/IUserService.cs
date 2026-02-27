using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces.User;

public interface IUserService
{
    //for authorization
    public Task<Result<AuthorizationResponseDto>> Authorization(AuthorizationDto authUser);
    public Task<Result<PrivateKeyResponseDto>> GetPrivateKey(Guid userId);
    
    //for registration
    public Task<Result<AuthorizationResponseDto>> Registration(RegistrationDto regUser);
    public Task<Result> AddKeys(AddKeysDto keys, Guid userId);
    
    //for profile
    public Task<Result<GetProfileDto>> GetProfile(Guid userId);
    public Task<Result<List<GetUserDto>>> GetBlockUsers(Guid userId);
    
    public Task<Result> Delete(Guid userId);
}