using AutoMapper;
using MediatR;
using Nexly.Application.Articles.DTOs;
using Nexly.Application.Core;

namespace Nexly.Application.Articles.Commands
{
    public class CreateArticle
    {
        public class Command: IRequest<Result<string>>
        {
            public required CreateArticleDto CreateArticleDto { get; set; }
        }

        public class Handler(NexlyDbContext context, IMapper mapper) : IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var article = mapper.Map<NewsArticle>(request.CreateArticleDto);

                context.NewsArticles.Add(article);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<string>.Failure("Failed to create article", 400);

                return Result<string>.Success(article.Id.ToString());
            }
        }
    }
}
