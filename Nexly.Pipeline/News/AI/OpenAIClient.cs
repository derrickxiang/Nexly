using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Nexly.Pipeline.News.AI
{
    public class OpenAIClient : IAIClient
    {
        private readonly HttpClient _http;
        private readonly AIClientOptions _options;

        public OpenAIClient(HttpClient http, AIClientOptions options)
        {
            _http = http;
            _options = options;
        }

        public async Task<string> GenerateAsync(
            string prompt,
            CancellationToken cancellationToken = default)
        {
            var request = BuildRequest(prompt);

            var response = await _http.PostAsync(
                "chat/completions",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()!;
        }

        public async Task<T> GenerateJsonAsync<T>(
            string prompt,
            CancellationToken cancellationToken = default)
        {
            var json = await GenerateAsync(prompt, cancellationToken);

            return JsonSerializer.Deserialize<T>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }

        private StringContent BuildRequest(string prompt)
        {
            var payload = new
            {
                model = _options.Model,
                messages = new[]
                {
                new { role = "user", content = prompt }
            },
                temperature = 0.2
            };

            var json = JsonSerializer.Serialize(payload);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
