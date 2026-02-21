using Nexly.Worker.Save.Messaging;

namespace Nexly.Worker.Save;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly RabbitMqListener _listener;

    public Worker(
        ILogger<Worker> logger,
        RabbitMqListener listener)
    {
        _logger = logger;
        _listener = listener;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Save Worker started");

        _listener.StartListening();

        return Task.CompletedTask;
    }
}
