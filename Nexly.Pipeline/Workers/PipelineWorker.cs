using Microsoft.Extensions.Hosting;
using Nexly.Pipeline.Contracts;
using Nexly.Pipeline.Messaging;
using Nexly.Pipeline.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Nexly.Pipeline.Workers
{
    public class PipelineWorker : BackgroundService
    {
        private readonly RabbitMqConsumer _consumer;
        private readonly RabbitMqPublisher _publisher;
        private readonly DedupService _dedup;
        private readonly CategoryService _category;

        public PipelineWorker(
            RabbitMqConsumer consumer,
            RabbitMqPublisher publisher,
            DedupService dedup,
            CategoryService category)
        {
            _consumer = consumer;
            _publisher = publisher;
            _dedup = dedup;
            _category = category;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Consume(async json =>
            {
                var raw =
                    JsonSerializer.Deserialize<RawNewsMessage>(json)!;

                if (_dedup.IsDuplicate(raw.Title, raw.Content))
                    return;

                var category =
                    _category.Detect(raw.Title);

                var aiMsg = new AiRequestMessage
                {
                    ArticleId = raw.Id,
                    Title = raw.Title,
                    Content = raw.Content,
                    Category = category,
                    Source = raw.Source,
                    PublishedAt = raw.PublishedAt
                };

                await _publisher.PublishAsync(
                    "ai.request",
                    aiMsg);
            });

            return Task.CompletedTask;
        }
    }
}
