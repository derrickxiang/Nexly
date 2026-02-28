using Nexly.Worker.AI.Extensions;
using Nexly.Worker.AI.Infrastructure;
using Nexly.Worker.AI.Workers;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.WithClientIp()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Ensure log directory exists
Directory.CreateDirectory("Logs");

// Replace default logging providers
builder.Logging.ClearProviders();
builder.Services.AddSerilog(Log.Logger);

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