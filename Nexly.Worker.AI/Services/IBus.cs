using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.AI.Services
{
    public interface IBus
    {
        void Subscribe(string queue, Func<string, Task> handler);
    }
}
