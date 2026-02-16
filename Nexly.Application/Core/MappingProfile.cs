using AutoMapper;
using Nexly.Application.Articles.DTOs;

namespace Nexly.Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<NewsArticle, NewsArticle>();
            CreateMap<NewsArticle, ArticleDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.ToString()));
            CreateMap<ArticleDto, NewsArticle>();
            CreateMap<CreateArticleDto, NewsArticle>()
                .ForMember(d => d.Id, o=> o.MapFrom(s => new Guid()));
        }
    }
}
