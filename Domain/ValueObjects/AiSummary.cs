using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Domain.ValueObjects;

public class AiSummary
{
    public string Summary { get; }
    public string Model { get; }
    public TokenUsage Usage { get; }

    public AiSummary(string summary, string model, TokenUsage usage)
    {
        Summary = summary;
        Model = model;
        Usage = usage;
    }
}
