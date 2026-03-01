using Nexly.Worker.AI.Extensions;
using Nexly.Worker.AI.Infrastructure;
using Nexly.Worker.AI.Workers;
using Serilog;
using Serilog.Debugging;

var builder = Host.CreateApplicationBuilder(args);

// Enable Serilog internal diagnostics (helps reveal config errors)
SelfLog.Enable(Console.Error);

// Create bootstrap logger
Log.Logger = new LoggerConfiguration()
    .Enrich.WithClientIp()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateBootstrapLogger();

// Ensure log directory exists
Directory.CreateDirectory("Logs");

// Wire Serilog into .NET logging
builder.Services.AddSerilog((services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.WithClientIp()
        .Enrich.FromLogContext();
});


// AI
builder.Services.AddAI(builder.Configuration);

// RabbitMQ
builder.Services.AddSingleton<RabbitMqConnection>();

// Worker
builder.Services.AddHostedService<AiWorker>();

var app = builder.Build();

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application crashed");
}
finally
{
    Log.CloseAndFlush();
}