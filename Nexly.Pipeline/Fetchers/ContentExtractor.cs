using HtmlAgilityPack;

public class ContentExtractor
{
    private readonly HttpClient _http;

    public ContentExtractor(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> ExtractAsync(string url)
    {
        var html = await _http.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var paragraphs = doc.DocumentNode.SelectNodes("//p");

        if (paragraphs == null)
            return "";

        return string.Join("\n",
            paragraphs.Select(p => p.InnerText));
    }
}
