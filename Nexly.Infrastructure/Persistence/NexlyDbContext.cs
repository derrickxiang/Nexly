using Microsoft.EntityFrameworkCore;
using Nexly.Domain.Entities;

namespace Nexly.Infrastructure.Persistence;

public class NexlyDbContext : DbContext
{
    public NexlyDbContext(DbContextOptions<NexlyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Source> Sources => Set<Source>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(NexlyDbContext).Assembly);
    }
}