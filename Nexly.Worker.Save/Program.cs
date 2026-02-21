using Nexly.Application;
using Nexly.Infrastructure;
using Nexly.Worker.Save;
using Nexly.Worker.Save.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

// ⭐ 注册 RabbitMQ Listener
builder.Services.AddSingleton<RabbitMqListener>();

// ⭐ Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
