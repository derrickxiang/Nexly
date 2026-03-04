using Nexly.Application.Interfaces.Services;

namespace Nexly.Infrastructure.Services;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}