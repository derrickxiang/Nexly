namespace Nexly.Application.Articles.DTOs
{
    public class ArticleDto
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public string TitleZh { get; set; }

        public string Content { get; set; }

        public string Summary { get; set; }
        public string SummaryZh { get; set; }

        public string SourceUrl { get; set; }
        public string SourceName { get; set; }

        public DateTime PublishedAt { get; set; }
        public DateTime CollectedAt { get; set; }

        public string Language { get; set; }

        public string Hash { get; set; }
        public string? ClusterId { get; set; }

        public float QualityScore { get; set; }
    }
}
