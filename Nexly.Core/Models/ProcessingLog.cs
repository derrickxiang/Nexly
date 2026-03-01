public class ProcessingLog
{
    public Guid Id { get; set; }
public Guid ArticleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ProcessName { get; set; }
    public string Stage { get; set; }
    public string Status { get; set; } // e.g., Started, Completed, Failed
    public string Details { get; set; }
}