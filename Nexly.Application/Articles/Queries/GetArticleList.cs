using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexly.Application.Articles.DTOs;
using Nexly.Application.Core;

namespace Nexly.Application.Articles.Queries
{
    public class GetArticleList
    {


        public class Query : IRequest<Result<List<ArticleDto>>>
        {
            public required ArticleParams Params { get; set; }
        }

        public class Handler(NexlyDbContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ArticleDto>>>
        {
            public async Task<Result<List<ArticleDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = await context.Articles
                    .OrderBy(x => x.PublishedAt)
                    .Where(x =>  request.Params.StartDate == DateTime.MinValue || x.PublishedAt >= (request.Params.Cursor ?? request.Params.StartDate))
                    .ToListAsync();

                //if (!string.IsNullOrEmpty(request.Params.Filter))
                //{
                //    query = request.Params.Filter switch
                //    {
                //        "isGoing" => query.Where(x => x.Attendees.Any(a => a.UserId == userAccessor.GetUserId())),
                //        "isHost" => query.Where(x => x.Attendees.Any(a => a.IsHost && a.UserId == userAccessor.GetUserId())),
                //        _ => query
                //    };
                //}

                
                //var activities = query
                //    .Take(request.Params.PageSize + 1);

               

                return Result<List<ArticleDto>>.Success(
                    mapper.Map<List<ArticleDto>>(query)
                    );
            }
        }
    }
}
