using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexly.Domain.Entities;

namespace Nexly.Infrastructure.Persistence.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(1000);

        builder.OwnsOne(x => x.AiSummary, summary =>
        {
            summary.Property(s => s.Summary)
                .HasColumnName("AiSummary");

            summary.OwnsOne(s => s.Usage, usage =>
            {
                usage.Property(u => u.PromptTokens)
                    .HasColumnName("PromptTokens");

                usage.Property(u => u.CompletionTokens)
                    .HasColumnName("CompletionTokens");

                usage.Property(u => u.EstimatedCost)
                    .HasColumnName("EstimatedCost");
            });
        });
    }
}