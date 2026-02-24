using MemoryTrave.Application.Dto.Responses.User;

namespace MemoryTrave.Application.Interfaces.Profile;

public interface IProfileService
{
    public Task<GetProfileDto> GetProfile();
    public Task<List<GetUserDto>> GetBlockUsers();
}