public class NewsArticleTopic
{
    public Guid NewsArticleId { get; set; }
    public NewsArticle NewsArticle { get; set; }

    public Guid TopicTagId { get; set; }
    public TopicTag TopicTag { get; set; }
}