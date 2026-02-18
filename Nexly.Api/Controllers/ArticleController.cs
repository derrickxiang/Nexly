using Microsoft.AspNetCore.Mvc;
using Nexly.Application.Articles.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexly.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            // TODO: Implement GetArticles logic
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticleById(int id)
        {
            // TODO: Implement GetArticleById logic
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Article>> CreateArticle([FromBody] CreateArticleDto request)
        {
            // TODO: Implement CreateArticle logic
            return CreatedAtAction(nameof(GetArticleById), new { id = 0 }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] CreateArticleDto request)
        {
            // TODO: Implement UpdateArticle logic
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            // TODO: Implement DeleteArticle logic
            return NoContent();
        }
    }
}