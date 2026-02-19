using Microsoft.AspNetCore.Mvc;
using Nexly.Application.Articles.Commands;
using Nexly.Application.Articles.DTOs;
using Nexly.Application.Articles.Queries;

namespace Nexly.Api.Controllers
{
    public class ArticleController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<ArticleDto>>> GetArticles([FromQuery] ArticleParams articleParams)
        {
            return HandleResult(await Mediator.Send(new GetArticleList.Query
            {
                Params = articleParams
            }));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ArticleDto>> GetArticleDetail(string Id)
        {
            return HandleResult(await Mediator.Send(new GetArticleDetails.Query { Id = Id }));
        }


        [HttpPost]
        public async Task<IActionResult> CreateArticle(CreateArticleDto articleDto)
        {
            return HandleResult(await Mediator.Send(new CreateArticle.Command { CreateArticleDto = articleDto }));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteArticle(string Id)
        {
            return HandleResult(await Mediator.Send(new DeleteArticle.Command { Id = Id }));
        }
    }
}
