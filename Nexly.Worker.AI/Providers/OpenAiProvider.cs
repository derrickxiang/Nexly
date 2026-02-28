using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Nexly.Worker.AI.Abstractions;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;

namespace Nexly.Worker.AI.Providers;

public class OpenAiProvider : IAiProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IAiUsageTracker _usageTracker;

    private readonly AsyncPolicy<HttpResponseMessage> _policy;

    public string Name => "OpenAI";

    public OpenAiProvider(
        HttpClient httpClient,
        IConfiguration config,
        IAiUsageTracker usageTracker)
    {
        _httpClient = httpClient;
        _config = config;
        _usageTracker = usageTracker;

        var retry = HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(r => r.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)));

        var breaker = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                5,
                TimeSpan.FromSeconds(30));

        _policy = Policy.WrapAsync(retry, breaker);
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
    {
        var apiKey = _config["AI:OpenAI:ApiKey"];

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var request = new
        {
            model = "gpt-4.1-mini",
            input = prompt
        };

        var response = await _policy.ExecuteAsync(() =>
            _httpClient.PostAsync(
                "https://api.openai.com/v1/responses",
                new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"),
                ct));

        var body = await response.Content.ReadAsStringAsync(ct);

        response.EnsureSuccessStatusCode();

        var json = JsonDocument.Parse(body);

        var text = json.RootElement
            .GetProperty("output")[0]
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString() ?? "";

        int inputTokens = json.RootElement
            .GetProperty("usage")
            .GetProperty("input_tokens")
            .GetInt32();

        int outputTokens = json.RootElement
            .GetProperty("usage")
            .GetProperty("output_tokens")
            .GetInt32();

        _usageTracker.Track(Name, inputTokens, outputTokens);

        return text;
    }
}