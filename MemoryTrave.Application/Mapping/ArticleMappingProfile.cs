using AutoMapper;
using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Dto.Responses.Article;
using MemoryTrave.Application.Dto.Responses.Article.GetArticle;
using MemoryTrave.Domain.Enums;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public class ArticleMappingProfile : Profile
{
    public ArticleMappingProfile()
    {
        CreateMap<Article, GetArticleForProfileDto>()
            .ForMember(dto => dto.LocationName,
                opt => opt.MapFrom(src =>
                    src.Location != null ? src.Location.Name : null))
            .ForMember(dto => dto.IsPrivate,
                opt => opt.MapFrom(src =>
                    src.Visibility == VisibilityEnum.Private))
            .ForMember(dto => dto.EncryptedPreviewData,
                opt => opt.MapFrom(src =>
                    src.Visibility == VisibilityEnum.Private ? src.EncryptedPreviewData : null))
            .ForMember(dto => dto.Description,
                opt => opt.MapFrom(src =>
                    src.Visibility == VisibilityEnum.Public ? src.Description : null));

        CreateMap<Article, GetArticleBaseDto>()
            .ForMember(dto => dto.AuthorName,
                opt => opt.MapFrom(src =>
                    src.Author.Username))
            .ForMember(dto => dto.LocationName,
                opt => opt.MapFrom(src =>
                    src.Location.Name));
        
        CreateMap<Article, GetPrivateArticleDto>()
            .IncludeBase<Article, GetArticleBaseDto>()
            .ForMember(dto => dto.EncryptedKey,
                opt => opt.Ignore());

        CreateMap<Article, GetPublicArticleDto>()
            .IncludeBase<Article, GetArticleBaseDto>();
        
        CreateMap<AddPrivateArticleDto, Article>()
            .ForMember(mod => mod.Id, opt => 
                opt.Ignore())
            .ForMember(mod => mod.Visibility, opt => 
                opt.Ignore())
            .ForMember(mod => mod.CreatedAt, opt => 
                opt.Ignore())
            .ForMember(mod => mod.LastChange, opt => 
                opt.Ignore())
            .ForMember(mod => mod.EncryptedKeys, opt =>
                opt.Ignore())
            .ForMember(mod => mod.Description, opt => 
                opt.Ignore())
            .ForMember(mod => mod.PhotosUrls, opt => 
                opt.Ignore())
            .ForMember(mod => mod.AuthorId, opt =>
                opt.Ignore());
        
        CreateMap<AddPublicArticleDto, Article>()
            .ForMember(mod => mod.Id, opt => 
                opt.Ignore())
            .ForMember(mod => mod.Visibility, opt => 
                opt.Ignore())
            .ForMember(mod => mod.CreatedAt, opt => 
                opt.Ignore())
            .ForMember(mod => mod.LastChange, opt => 
                opt.Ignore())
            .ForMember(mod => mod.EncryptedPreviewData, opt => 
                opt.Ignore())
            .ForMember(mod => mod.EncryptedData, opt => 
                opt.Ignore())
            .ForMember(mod => mod.EncryptedKeys, opt => 
                opt.Ignore())
            .ForMember(mod => mod.AuthorId, opt =>
                opt.Ignore());
        
        CreateMap<UpdateArticleDto, Article>()
            .ForMember(mod => mod.Id, opt => 
                opt.Ignore())
            .ForMember(mod => mod.CreatedAt, opt => 
                opt.Ignore())
            .ForMember(mod => mod.EncryptedKeys, opt => 
                opt.Ignore())
            .ForMember(mod => mod.AuthorId, opt =>
                opt.Ignore())
            .ForMember(mod => mod.LocationId, opt =>
                opt.Ignore());
    }
}