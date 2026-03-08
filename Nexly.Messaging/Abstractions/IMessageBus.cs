using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Messaging.Abstractions
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message, CancellationToken ct);

        void Subscribe<T, THandler>()
        where THandler : IMessageHandler<T>;
    }
}
