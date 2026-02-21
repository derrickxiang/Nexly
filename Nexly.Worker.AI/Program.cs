using Nexly.Worker.AI.Extensions;
using Nexly.Worker.AI.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAI();
builder.Services.AddHostedService<AiWorker>();

var app = builder.Build();
app.Run();