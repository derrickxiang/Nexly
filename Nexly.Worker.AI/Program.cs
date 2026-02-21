using Nexly.Worker.AI.Extensions;
using Nexly.Worker.AI.Services;
using Nexly.Worker.AI.Workers;
using Polly;
using Polly.Extensions.Http;
using System.Net;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAI();
builder.Services.AddHostedService<AiWorker>();

builder.Services.AddHttpClient<IAiService, OpenAiService>()
    .AddPolicyHandler(GetRetryPolicy());

var app = builder.Build();
app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
        .WaitAndRetryAsync(
            5,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (outcome, timespan, retryAttempt, context) =>
            {
                Console.WriteLine($"Retry {retryAttempt} after {timespan}");
            });
}