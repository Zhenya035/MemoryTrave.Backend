using MemoryTrave.Application.Dto;
using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces;

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

    public Task<Result<GetPublicKeysDto>> GetPublicKey(Guid userId);
    
    public Task<Result<List<GetUserDto>>> GetUsersWithoutMe(Guid userId);
    public Task<Result> Block(ListIdDto blockIds, Guid userId);
    public Task<Result> Unblock(ListIdDto unblockIds, Guid userId);
    public Task<Result> Delete(Guid userId);
}