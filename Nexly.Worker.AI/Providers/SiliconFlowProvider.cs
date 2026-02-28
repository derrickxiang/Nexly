using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Nexly.Worker.AI.Abstractions;
using Polly;
using Polly.Extensions.Http;

namespace Nexly.Worker.AI.Providers;

public class SiliconFlowProvider : IAiProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IAiUsageTracker _usageTracker;
    private readonly AsyncPolicy<HttpResponseMessage> _policy;

    public string Name => "SiliconFlow";

    public SiliconFlowProvider(
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
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var breaker = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                5,
                TimeSpan.FromSeconds(30));

        _policy = Policy.WrapAsync(retry, breaker);
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
    {
        var apiKey = _config["AI:SiliconFlow:ApiKey"];

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var request = new
        {
            model = "Pro/zai-org/GLM-4.7",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var response = await _policy.ExecuteAsync(() =>
            _httpClient.PostAsync(
                "https://api.siliconflow.cn/v1/chat/completions",
                new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"),
                ct));

        var body = await response.Content.ReadAsStringAsync(ct);

        response.EnsureSuccessStatusCode();

        var json = JsonDocument.Parse(body);

        var text = json.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "";

        // SiliconFlow 可能没有 usage，安全处理
        int inputTokens = 0;
        int outputTokens = 0;

        if (json.RootElement.TryGetProperty("usage", out var usage))
        {
            if (usage.TryGetProperty("prompt_tokens", out var pt))
                inputTokens = pt.GetInt32();

            if (usage.TryGetProperty("completion_tokens", out var ctokens))
                outputTokens = ctokens.GetInt32();
        }

        _usageTracker.Track(Name, inputTokens, outputTokens);

        return text;
    }
}