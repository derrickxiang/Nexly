namespace Nexly.Messaging.Abstractions;

public interface IMessageHandler<T>
{
    Task HandleAsync(
        T message,
        CancellationToken ct);
}