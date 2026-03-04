using Microsoft.EntityFrameworkCore;
using Nexly.Application.Interfaces.Repositories;
using Nexly.Domain.Entities;

namespace Nexly.Infrastructure.Persistence.Repositories;

public class SourceRepository : ISourceRepository
{
    private readonly NexlyDbContext _context;

    public SourceRepository(NexlyDbContext context)
    {
        _context = context;
    }

    public async Task<List<Source>> GetActiveSourcesAsync(CancellationToken ct)
    {
        return await _context.Sources
            .Where(x => x.IsActive)
            .ToListAsync(ct);
    }
}