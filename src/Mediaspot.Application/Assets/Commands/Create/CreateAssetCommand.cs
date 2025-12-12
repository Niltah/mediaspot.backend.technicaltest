using MediatR;

namespace Mediaspot.Application.Assets.Commands.Create;

public abstract record CreateAssetCommand(string ExternalId, string Title, string? Description, string? Language)
    : IRequest<Guid>;