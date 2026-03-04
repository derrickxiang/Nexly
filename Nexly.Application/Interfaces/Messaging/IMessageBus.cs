namespace Nexly.Application.Interfaces.Messaging;

public interface IMessageBus
{
    Task PublishAsync<T>(T message, CancellationToken ct);
}