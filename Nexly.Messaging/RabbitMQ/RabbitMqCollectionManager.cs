using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Nexly.Messaging.RabbitMQ;

public class RabbitMqConnectionManager : IDisposable
{
    private readonly IConnection _connection;

    public RabbitMqConnectionManager(
        IOptions<RabbitMqOptions> options)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.Value.HostName,
            UserName = options.Value.UserName,
            Password = options.Value.Password,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
    }

    public IModel CreateChannel()
        => _connection.CreateModel();

    public void Dispose()
        => _connection.Dispose();
}