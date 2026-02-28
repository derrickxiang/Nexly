using Nexly.Worker.AI.Abstractions;

namespace Nexly.Worker.AI.Infrastructure;

public class AiUsageTracker : IAiUsageTracker
{
    private readonly ILogger<AiUsageTracker> _logger;

    private static long _totalInput;
    private static long _totalOutput;

    // 你可以改成真实费率
    private const decimal OpenAiInputCost = 0.000002m;
    private const decimal OpenAiOutputCost = 0.000006m;

    public AiUsageTracker(ILogger<AiUsageTracker> logger)
    {
        _logger = logger;
    }

    public void Track(string provider, int inputTokens, int outputTokens)
    {
        Interlocked.Add(ref _totalInput, inputTokens);
        Interlocked.Add(ref _totalOutput, outputTokens);

        decimal cost = 0;

        if (provider == "OpenAI")
        {
            cost =
                inputTokens * OpenAiInputCost +
                outputTokens * OpenAiOutputCost;
        }

        _logger.LogInformation(
            "Provider={Provider} | Input={Input} | Output={Output} | Cost=${Cost}",
            provider,
            inputTokens,
            outputTokens,
            cost);
    }
}