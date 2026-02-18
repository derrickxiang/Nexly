using AutoMapper;
using Nexly.Application.Articles.DTOs;

namespace Nexly.Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Article, Article>();
            CreateMap<Article, ArticleDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.ToString()));
            CreateMap<ArticleDto, Article>();
            CreateMap<CreateArticleDto, Article>()
                .ForMember(d => d.Id, o=> o.MapFrom(s => new Guid()));
        }
    }
}
