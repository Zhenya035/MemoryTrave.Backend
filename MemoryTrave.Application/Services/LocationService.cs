using AutoMapper;
using Geohash;
using MemoryTrave.Application.Dto.Requests.Location;
using MemoryTrave.Application.Dto.Responses.Location;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Domain.Common;
using MemoryTrave.Domain.Enums;
using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Services;

public class LocationService(
    ILocationRepository repository,
    IUserRepository userRepository,
    IValidationService validationService,
    IMapper mapper) : ILocationService
{
    private const int CountryGeoHashSize = 3;
    private const int CityGeoHashSize = 6;
    private const int PointGeoHashSize = 8;
    
    public Task<Result<List<GetAllLocationDto>>> GetAll(GetLocationRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<GetLocationDto>> GetById(Guid locationId, Guid userId)
    {
        if (locationId == Guid.Empty)
            return Result<GetLocationDto>.Failure("Invalid location id", ErrorCode.InvalidInput);
        
        var isExist = await repository.Exists(locationId);
        if (!isExist)
            return Result<GetLocationDto>.Failure("Location not found", ErrorCode.NotFound);
        
        isExist = await userRepository.ExistsById(userId);
        if (!isExist)
            return Result<GetLocationDto>.Failure("User not found", ErrorCode.NotFound);
        
        var location = await repository.Get(locationId, userId); 
        
        var response = mapper.Map<GetLocationDto>(location);
        
        foreach (var article in response.Articles)
        {
            if (article.Visibility == VisibilityEnum.Private)
            {
               var key = location.Articles
                    .FirstOrDefault(a => a.Id == article.Id)?
                    .EncryptedKeys?
                    .FirstOrDefault()?
                    .EncryptedKey;
            
                article.EncryptedKey = key;
            }
        }
        
        return Result<GetLocationDto>.Success(response);
    }

    public async Task<Result> Add(AddAndUpdateLocationDto dto)
    {
        var validResult = await validationService.Validate(dto);
        if (!validResult.IsSuccess)
            return Result.Failure(validResult.Error,  ErrorCode.InvalidInput);

        var location = mapper.Map<Location>(dto);
        location.Id = Guid.NewGuid();
        var geohasher = new Geohasher();
        location.Geohash = location.Type switch
        {
            LocationTypeEnum.Country => geohasher.Encode(location.Latitude, location.Longitude, CountryGeoHashSize),
            LocationTypeEnum.City => geohasher.Encode(location.Latitude, location.Longitude, CityGeoHashSize),
            LocationTypeEnum.Point => geohasher.Encode(location.Latitude, location.Longitude, PointGeoHashSize),
            _ => throw new Exception("Invalid location type")
        };

        await repository.Add(location);
        return Result.Success();
    }

    public async Task<Result> Update(AddAndUpdateLocationDto dto, Guid locationId)
    {
        var validResult = await validationService.Validate(dto);
        if (!validResult.IsSuccess)
            return Result.Failure(validResult.Error,  ErrorCode.InvalidInput);
        
        if (locationId == Guid.Empty)
            return Result.Failure("Invalid locationId", ErrorCode.InvalidInput);
        
        var isExist = await repository.Exists(locationId);
        if (!isExist)
            return Result.Failure("Location not found", ErrorCode.NotFound);

        var location = mapper.Map<Location>(dto);
        await repository.Update(location, locationId);
        return Result.Success();
    }

    public async Task<Result> Delete(Guid locationId)
    {
        if (locationId == Guid.Empty)
            return Result.Failure("Invalid locationId", ErrorCode.InvalidInput);
        
        var isExist = await repository.Exists(locationId);
        if (!isExist)
            return Result.Failure("Location not found", ErrorCode.NotFound);
        
        await  repository.Delete(locationId);
        return Result.Success();
    }
}