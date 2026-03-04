using RabbitMQ.Client;

namespace Nexly.Messaging.RabbitMQ;

public class RabbitMqConnectionManager : IDisposable
{
    private readonly IConnection _connection;

    public RabbitMqConnectionManager(RabbitMqOptions options)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.HostName,
            UserName = options.UserName,
            Password = options.Password
        };

        _connection = factory.CreateConnection();
    }

    public IModel CreateChannel()
        => _connection.CreateModel();

    public void Dispose()
        => _connection.Dispose();
}