public class AIResult
{
    public Guid Id { get; set; }

    public Guid ArticleId { get; set; }
    public string Model { get; set; } // e.g., GPT-4, Claude
    public int TokenUsed { get; set; }
    public int LatencyMs { get; set; }
    public DateTime CreatedAt { get; set; }
}