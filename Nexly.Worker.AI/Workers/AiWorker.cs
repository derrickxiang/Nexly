using Nexly.Worker.AI.Abstractions;
using Nexly.Worker.AI.Models;
using Nexly.Worker.AI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Nexly.Worker.AI.Workers
{

    public class AiWorker : BackgroundService
    {
        private readonly ILogger<AiWorker> _logger;
        private readonly IBus _bus;
        private readonly IAiProvider _aiService;

        public AiWorker(
            ILogger<AiWorker> logger,
            IBus bus,
            IAiProvider aiService)
        {
            _logger = logger;
            _bus = bus;
            _aiService = aiService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus.Subscribe("ai.jobs", async message =>
            {
                try
                {
                    var job = JsonSerializer.Deserialize<AiJobMessage>(message);

                    if (job == null) return;

                    _logger.LogInformation("Processing AI Job: {Id}", job.NewsId);

                    var result = await _aiService.GenerateAsync(job.Content);

                    _logger.LogInformation("AI Done: {Id}", job.NewsId);

                    await Task.Delay(1000);

                    // TODO: Save to DB API or publish event
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "AI Worker Error");
                }
             });

            return Task.CompletedTask;
        }
    }

}
