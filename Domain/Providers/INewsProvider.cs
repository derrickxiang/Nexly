using Nexly.Domain.Entities;
using Nexly.Domain.Models;

namespace Nexly.Domain.Repositories;

public interface INewsProvider
{
    Task<IEnumerable<Article>> FetchLatestAsync(
        CancellationToken ct);
}