using MemoryTrave.Application.Dto.Requests.Location;
using MemoryTrave.Application.Dto.Responses.Location;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces;

public interface ILocationService
{
    public Task<Result<List<GetAllLocationDto>>> GetAll(Guid userId);
    public Task<Result<GetLocationDto>> GetById(Guid locationId, Guid userId);
    
    public Task<Result> Add(AddAndUpdateLocationDto dto);
    
    public Task<Result> Update(AddAndUpdateLocationDto dto, Guid locationId);
    
    public Task<Result> Delete(Guid locationId);
}