using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Domain.ValueObjects;

public class TokenUsage
{
    public int PromptTokens { get; }
    public int CompletionTokens { get; }
    public decimal EstimatedCost { get; }

    public TokenUsage(int promptTokens, int completionTokens, decimal estimatedCost)
    {
        PromptTokens = promptTokens;
        CompletionTokens = completionTokens;
        EstimatedCost = estimatedCost;
    }
}
