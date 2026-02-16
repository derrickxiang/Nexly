using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexly.Application.Articles.DTOs;
using Nexly.Application.Core;

namespace Nexly.Application.Articles.Queries
{
    public class GetArticleDetails
    {
        public class Query: IRequest<Result<ArticleDto>>
        {
            public required string Id { get; set; }
        }

        public class Handler(NexlyDbContext context, IMapper mapper) : IRequestHandler<Query, Result<ArticleDto>>
        {
            public async Task<Result<ArticleDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var article = await context.NewsArticles.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.Id));

                if (article == null) return Result<ArticleDto>.Failure("Article not found", 404);

                return Result<ArticleDto>.Success(mapper.Map<ArticleDto>(article));
            }
        }
    }
}
