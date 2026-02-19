using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Messaging.Interfaces
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(string queue, T message);

        void Subscribe<T>(
            string queue,
            Func<T, Task> handler);
    }
}
