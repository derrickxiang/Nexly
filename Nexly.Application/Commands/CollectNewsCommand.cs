using MediatR;

namespace Nexly.Application.Commands;

public record CollectNewsCommand() : IRequest<int>;