using FluentValidation;
using Nexly.Application.Articles.DTOs;

namespace Nexly.Application.Articles.Validators
{
    public class BaseArticleValidator<T, TDto> : AbstractValidator<T> where TDto : BaseArticleDto
    {
        public BaseArticleValidator(Func<T, TDto> selector)
        {
            RuleFor(x => selector(x).Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not extend 200 chars");
            RuleFor(x => selector(x).PublishedAt)
                .LessThan(DateTime.UtcNow).WithMessage("Date must be in the past");
        }
    }
}
