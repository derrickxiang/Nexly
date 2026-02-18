public class TopicTag
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; } // e.g., SG / Global / Tech
    public ICollection<ArticleTopic> Articles { get; set; }
}