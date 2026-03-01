public class ArticleTopic
{
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }

    public Guid TopicTagId { get; set; }
    public TopicTag TopicTag { get; set; }
}