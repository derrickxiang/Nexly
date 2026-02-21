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

        private const string ApiKey = "You-api-key";
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
