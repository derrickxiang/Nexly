using Microsoft.Extensions.Hosting;
using Nexly.Application;
using Nexly.Infrastructure;
using Nexly.Messaging;
using Nexly.Worker.Jobs;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddMessaging(builder.Configuration);

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("CollectNewsJob");

    q.AddJob<CollectNewsJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("collect-news-trigger")
        .WithCronSchedule("0 */5 * * * ?"));
});

builder.Services.AddQuartzHostedService();

var host = builder.Build();
host.Run();