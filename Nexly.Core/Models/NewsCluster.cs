public class NewsCluster
{
    public Guid Id { get; set; }

    public string CanoicalTitle { get; set; }
    public string Topic { get; set; }
    public DateTime CreatedAt { get; set; }

    public int ArticleCount { get; set; }
}