using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.AI.Abstractions
{
    public interface IAiProvider
    {
        string Name { get; }

        Task<string> GenerateAsync(string prompt, CancellationToken ct = default);
    }
}
