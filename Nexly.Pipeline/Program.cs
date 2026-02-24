using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexly.Pipeline.Messaging;
using Nexly.Pipeline.News.AI;
using Nexly.Pipeline.News.Collectors;
using Nexly.Pipeline.News.Worker;
using Nexly.Pipeline.Services;
using Nexly.Pipeline.Workers;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<RabbitMqConsumer>();
builder.Services.AddSingleton<RabbitMqPublisher>();

builder.Services.AddSingleton<DedupService>();
builder.Services.AddSingleton<CategoryService>();

builder.Services.AddHostedService<PipelineWorker>();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<INewsProvider, RssNewsProvider>();
builder.Services.AddSingleton<INewsProvider, GoogleNewsProvider>();

builder.Services.AddSingleton<INewsCollector, NewsCollector>();

var options = builder.Configuration
    .GetSection("AI")
    .Get<AIClientOptions>();

builder.Services.AddSingleton(options);

builder.Services.AddHttpClient<IAIClient, OpenAIClient>((sp, client) =>
{
    var options = sp.GetRequiredService<AIClientOptions>();

    client.BaseAddress = new Uri(options.BaseUrl);

    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", options.ApiKey);

    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
});


builder.Services.AddHostedService<NewsCollectorWorker>();

var app = builder.Build();

app.Run();
