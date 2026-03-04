using Nexly.Domain.Entities;

namespace Nexly.Application.Interfaces.Repositories;

public interface ISourceRepository
{
    Task<List<Source>> GetActiveSourcesAsync(CancellationToken ct);
}