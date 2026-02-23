using Nexly.Application.Interfaces;
using Nexly.Pipeline.News.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.Steps
{
    public class PersistNewsStep
    {
        private readonly INewsRepository _repository;

        public PersistNewsStep(INewsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> ExecuteAsync(AIEnrichedNews input)
        {
            var news = new Nexly.Domain.News
            {
                Id = Guid.NewGuid(),
                Title = input.Title,
                Summary = input.Summary,
                Content = input.Content,
                Url = input.Url,
                Source = input.Source,
                Category = input.Category,
                ImportanceScore = input.ImportanceScore,
                PublishedAt = input.PublishedAt
            };

             _repository.Create(news);

            return news.Id;
        }
    }
}
