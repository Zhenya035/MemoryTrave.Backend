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
    IFriendshipRepository  friendshipRepository,
    IValidationService validationService,
    IMapper mapper) : ILocationService
{
    private const int CountryGeoHashSize = 3;
    private const int CityGeoHashSize = 6;
    private const int PointGeoHashSize = 8;
    
    public async Task<Result<List<GetAllLocationDto>>> GetAll(Guid userId)
    {
        var locations = await repository.GetAll();

        var friends = new List<Guid>();
        if(userId != Guid.Empty)
             friends = await friendshipRepository.GetAllFriendsIds(userId);
        
        var result = new List<GetAllLocationDto>();
        
        foreach (var loc in locations)
        {
            var locDto = mapper.Map<GetAllLocationDto>(loc);
            
            var authorIds = loc.Articles.Select(a => a.AuthorId).ToHashSet();

            if (authorIds.Count == 0)
                locDto.LocationContentState = LocationContentState.Empty;
            else
            {
                var hasUser = authorIds.Contains(userId);
                var hasFriends = authorIds.Any(friends.Contains);

                locDto.LocationContentState = (hasUser, hasFriends) switch
                {
                    (true, true) => LocationContentState.MyAndFriendsArticles,
                    (true, false) => LocationContentState.MyArticles,
                    (false, true) => LocationContentState.FriendsArticles,
                    _ => LocationContentState.OtherArticles
                };
            }
            
            result.Add(locDto);
        }
        
        return Result<List<GetAllLocationDto>>.Success(result);
    }
    
    public async Task<Result<GetLocationDto>> GetById(Guid locationId, Guid userId)
    {
        if (locationId == Guid.Empty)
            return Result<GetLocationDto>.Failure("Invalid location id", ErrorCode.InvalidInput);
        
        var isExist = await repository.Exists(locationId);
        if (!isExist)
            return Result<GetLocationDto>.Failure("Location not found", ErrorCode.NotFound);
        
        Location? location;
        if(userId != Guid.Empty)
        {
            isExist = await userRepository.ExistsById(userId);
            if (!isExist)
                return Result<GetLocationDto>.Failure("User not found", ErrorCode.NotFound);
            
            location = await repository.GetForUser(locationId, userId);
        }
        else
            location = await repository.GetPublic(locationId);
        
        var response = mapper.Map<GetLocationDto>(location);
        
        foreach (var article in response.Articles)
        {
            if (article.Visibility == VisibilityEnum.Private)
            {
               var key = location.Articles
                    .FirstOrDefault(a => a.Id == article.Id)?
                    .EncryptedKeys?
                    .FirstOrDefault(k => k.UserId == userId)?
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