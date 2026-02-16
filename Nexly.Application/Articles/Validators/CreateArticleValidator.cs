using Nexly.Application.Articles.Commands;
using Nexly.Application.Articles.DTOs;

namespace Nexly.Application.Articles.Validators
{
    public sealed class CreateArticleValidator : BaseArticleValidator<CreateArticle.Command, CreateArticleDto>
    {
        public CreateArticleValidator() : base(x => x.CreateArticleDto)
        { }
    }
}
