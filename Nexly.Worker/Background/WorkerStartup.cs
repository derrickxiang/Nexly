using Microsoft.Extensions.Hosting;
using Nexly.Messaging.Abstractions;
using Nexly.Messaging.Contracts;
using Nexly.Worker.Handlers;

namespace Nexly.Worker.Background;

public class WorkerStartup : IHostedService
{
    private readonly IMessageBus _bus;

    public WorkerStartup(IMessageBus bus)
    {
        _bus = bus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _bus.Subscribe<ArticleCollectedEvent, ArticleCollectedHandler>();
        _bus.Subscribe<ArticleProcessedEvent, ArticleProcessedHandler>();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}