using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Nexly.Worker.AI.Services
{
    public class OpenAiService : IAiService
    {
        private readonly HttpClient _http;

        private const string ApiKey = "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";
        private const string Endpoint = "https://api.openai.com/v1/responses";

        public OpenAiService(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        }

        public async Task<AiResult> ProcessAsync(string content)
        {
            var prompt = $"""
        Summarize this news in English (3 sentences)
        Then translate to Chinese.

        News:
        {content}
        """;

            var req = new
            {
                model = "gpt-4.1-mini",
                input = prompt
            //    messages = new[]
            //    {
            //    new { role = "user", content = prompt }
            //}
            };

            var response = await _http.PostAsJsonAsync(Endpoint, req);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            var text = json
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            Console.WriteLine(text);

            return new AiResult
            {
                Summary = text ?? "",
                Chinese = text ?? ""
            };
        }
    }
}
