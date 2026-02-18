public static class PromptBuilder
{
    public static string BuildArticlePrompt(
        string title,
        string content)
    {
        return $"""
You are a professional news editor.

Analyze the following news and produce:

1. A concise English summary (120 words)
2. A fluent Chinese summary (120 words)
3. An optimized English headline
4. A Chinese headline

Requirements:

- Neutral journalistic tone
- No hallucination
- Keep factual accuracy
- Highlight key facts

NEWS TITLE:
{title}

NEWS CONTENT:
{content}
""";
    }
}
