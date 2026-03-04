namespace Nexly.Application.Interfaces.Services;

public interface IClock
{
    DateTime UtcNow { get; }
}