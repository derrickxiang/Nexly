public interface IAIService
{
    Task<AiResult> ProcessArticleAsync(string title, string content);
}

public class AiResult
{
    public string SummaryEn { get; set; }
    public string SummaryZh { get; set; }

    public string TitleEn { get; set; }
    public string TitleZh { get; set; }
}
