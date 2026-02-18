using MediatR;
using Nexly.Application.Core;

namespace Nexly.Application.Articles.Commands
{
    public class DeleteArticle
    {
        public class Command: IRequest<Result<Unit>>
        {
            public required string Id { get; set; }
        }

        public class Handler(NexlyDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var article = await context.Articles.FindAsync([Guid.Parse(request.Id)], cancellationToken);

                if (article == null) return Result<Unit>.Failure("article not found", 404);

                context.Remove(article);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete the article", 400);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
