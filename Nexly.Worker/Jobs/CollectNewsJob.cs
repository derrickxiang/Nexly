using MediatR;
using Quartz;
using Nexly.Application.Commands;

namespace Nexly.Worker.Jobs;

public class CollectNewsJob : IJob
{
    private readonly IMediator _mediator;

    public CollectNewsJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new CollectNewsCommand());
    }
}