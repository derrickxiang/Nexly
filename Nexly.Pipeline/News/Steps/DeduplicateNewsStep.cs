using Nexly.Application.Interfaces;
using Nexly.Pipeline.News.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.Steps
{
    public class DeduplicateNewsStep
    {
        private readonly INewsRepository _repository;

        public DeduplicateNewsStep(INewsRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExecuteAsync(NormalizedNews news)
        {
            var exists = await _repository.ExistsByHashAsync(news.Hash);

            return !exists;
        }
    }
}
