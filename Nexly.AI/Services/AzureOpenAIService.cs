using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;

public class AzureOpenAIService : IAIService
{
    private readonly OpenAIClient _client;
    private readonly string _deployment;

    public AzureOpenAIService(IConfiguration config)
    {
        var endpoint = config["AzureOpenAI:Endpoint"];
        var key = config["AzureOpenAI:Key"];
        _deployment = config["AzureOpenAI:Deployment"];

        _client = new OpenAIClient(
            new Uri(endpoint),
            new AzureKeyCredential(key));
    }

    public async Task<AiResult> ProcessNewsAsync(
        string title,
        string content)
    {
        var prompt =
            PromptBuilder.BuildNewsPrompt(title, content);

        var response =
            await _client.GetChatCompletionsAsync(
                _deployment,
                new ChatCompletionsOptions
                {
                    Messages =
                    {
                        new ChatMessage(
                            ChatRole.User,
                            prompt)
                    },
                    Temperature = 0.3f
                });

        var text =
            response.Value.Choices[0].Message.Content;

        return ParseResult(text);
    }

    private AiResult ParseResult(string text)
    {
        // 简单解析，可后续升级JSON模式
        return new AiResult
        {
            SummaryEn = Extract(text, "English summary"),
            SummaryZh = Extract(text, "Chinese summary"),
            TitleEn = Extract(text, "English headline"),
            TitleZh = Extract(text, "Chinese headline")
        };
    }

    private string Extract(string text, string key)
    {
        return text; // 先简化，后面升级
    }
}
