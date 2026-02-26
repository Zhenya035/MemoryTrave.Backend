using AutoMapper;
using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, GetProfileDto>()
            .ForMember(dto => dto.Role,
                opt => opt.MapFrom(src =>
                    src.Role.ToString()))
            .ForMember(dto => dto.ArticlesCount,
                opt => opt.MapFrom(src => 
                    src.Articles.Count))
            .ForMember(dto => dto.Articles,
                opt => opt.MapFrom(src =>
                    src.Articles));
        CreateMap<User, GetUserDto>();

        CreateMap<RegistrationDto, User>()
            .ForMember(model => model.Id, 
                opt => opt.Ignore())
            .ForMember(model => model.PasswordHash, 
                opt => opt.Ignore())
            .ForMember(model => model.PublicKey, 
                opt => opt.Ignore())
            .ForMember(model => model.EncryptedPrivateKey, 
                opt => opt.Ignore())
            .ForMember(model => model.Role, 
                opt => opt.Ignore())
            .ForMember(model => model.BlockedUsers, 
                opt => opt.Ignore())
            .ForMember(model => model.BanExpiresAt, 
                opt => opt.Ignore());
    }
}