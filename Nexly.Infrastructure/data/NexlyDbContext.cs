using Microsoft.EntityFrameworkCore;

public class NexlyDbContext : DbContext
{
    public NexlyDbContext(DbContextOptions<NexlyDbContext> options) : base(options)
    {
    }

    public DbSet<Article> Articles { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<TopicTag> TopicTags { get; set; }
    public DbSet<ArticleTopic> ArticleTopics { get; set; }
    public DbSet<NewsCluster> NewsClusters { get; set; }
    public DbSet<ProcessingLog> ProcessingLogs { get; set; }
    public DbSet<AIResult> AIResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ArticleTopic>()
            .HasKey(nat => new { nat.ArticleId, nat.TopicTagId });

        modelBuilder.Entity<ArticleTopic>()
            .HasOne(nat => nat.Article)
            .WithMany(na => na.Topics)
            .HasForeignKey(nat => nat.ArticleId);

        modelBuilder.Entity<ArticleTopic>()
            .HasOne(nat => nat.TopicTag)
            .WithMany(tt => tt.Articles)
            .HasForeignKey(nat => nat.TopicTagId);

        modelBuilder.Entity<Article>()
            .HasIndex(x => x.Hash);

        modelBuilder.Entity<Article>()
            .HasIndex(x => x.PublishedAt);

        modelBuilder.Entity<Article>()
            .HasIndex(x => x.ClusterId);
    }
}