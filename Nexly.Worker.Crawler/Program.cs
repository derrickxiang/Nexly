using Nexly.Worker.Crawler;
using Nexly.Worker.Crawler.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddCrawler();

var app = builder.Build();

app.Run();
