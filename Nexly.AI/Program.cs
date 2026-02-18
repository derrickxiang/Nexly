using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nexly.AI.Interfaces;
using Nexly.AI.Services;

var builder = Host.CreateApplicationBuilder(args);

// =========================
// Configuration
// =========================
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables();

// =========================
// Logging
// =========================
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// =========================
// Services
// =========================
builder.Services.AddHttpClient();

// Azure OpenAI Service
builder.Services.AddSingleton<IAIService, AzureOpenAIService>();

var app = builder.Build();


// =========================
// Test Run (Optional)
// =========================
using var scope = app.Services.CreateScope();

var ai = scope.ServiceProvider.GetRequiredService<IAIService>();

var result = await ai.ProcessNewsAsync(
    "Sample News Title",
    "This is a sample news content about technology and economy developments.");

Console.WriteLine("====== AI RESULT ======");
Console.WriteLine($"EN Title: {result.TitleEn}");
Console.WriteLine($"ZH Title: {result.TitleZh}");
Console.WriteLine($"EN Summary: {result.SummaryEn}");
Console.WriteLine($"ZH Summary: {result.SummaryZh}");

await app.RunAsync();
