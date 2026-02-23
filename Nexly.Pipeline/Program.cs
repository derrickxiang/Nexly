using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexly.Pipeline.Messaging;
using Nexly.Pipeline.News.Collectors;
using Nexly.Pipeline.News.Worker;
using Nexly.Pipeline.Services;
using Nexly.Pipeline.Workers;
using System;
using System.Collections.Generic;
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

builder.Services.AddHostedService<NewsCollectorWorker>();

var app = builder.Build();

app.Run();
