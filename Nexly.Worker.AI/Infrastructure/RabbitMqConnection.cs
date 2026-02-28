using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;

namespace Nexly.Worker.AI.Infrastructure;

public class RabbitMqConnection : IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    public IModel Channel { get; }

    public RabbitMqConnection(IConfiguration config)
    {
        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMQ:Host"],
            Port = int.Parse(config["RabbitMQ:Port"]!),
            UserName = config["RabbitMQ:Username"],
            Password = config["RabbitMQ:Password"],
           
        };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;

        _channel.QueueDeclareAsync(
            queue: config["RabbitMQ:Queue"],
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _channel.BasicQosAsync(0, 5, false); // 限制并发
    }

    public void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }
}