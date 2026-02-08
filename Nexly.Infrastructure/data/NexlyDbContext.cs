using Microsoft.EntityFrameworkCore;

public class NexlyDbContext : DbContext
{
    public NexlyDbContext(DbContextOptions<NexlyDbContext> options) : base(options)
    {
    }

    public DbSet<NewsArticle> NewsArticles { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<TopicTag> TopicTags { get; set; }
    public DbSet<NewsArticleTopic> NewsArticleTopics { get; set; }
    public DbSet<NewsCluster> NewsClusters { get; set; }
    public DbSet<ProcessingLog> ProcessingLogs { get; set; }
    public DbSet<AIResult> AIResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<NewsArticleTopic>()
            .HasKey(nat => new { nat.NewsArticleId, nat.TopicTagId });

        modelBuilder.Entity<NewsArticleTopic>()
            .HasOne(nat => nat.NewsArticle)
            .WithMany(na => na.Topics)
            .HasForeignKey(nat => nat.NewsArticleId);

        modelBuilder.Entity<NewsArticleTopic>()
            .HasOne(nat => nat.TopicTag)
            .WithMany(tt => tt.Articles)
            .HasForeignKey(nat => nat.TopicTagId);

        modelBuilder.Entity<NewsArticle>()
            .HasIndex(x => x.Hash);

        modelBuilder.Entity<NewsArticle>()
            .HasIndex(x => x.PublishedAt);

        modelBuilder.Entity<NewsArticle>()
            .HasIndex(x => x.ClusterId);
    }
}