using FluentValidation;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;
using MediatR;

namespace Mediaspot.Application.Assets.Commands.Create;

public sealed class CreateAssetHandler(IAssetRepository repo, IValidator<CreateAssetCommand> validator, IUnitOfWork uow)
    : IRequestHandler<CreateAssetCommand, Guid>
{
    public async Task<Guid> Handle(CreateAssetCommand request, CancellationToken ct)
    {
        // Validatre parameter
        validator.ValidateAndThrow(request);

        // Enforce uniqueness of ExternalId
        var existing = await repo.GetByExternalIdAsync(request.ExternalId, ct);
        if (existing is not null)
            throw new InvalidOperationException($"Asset with ExternalId '{request.ExternalId}' already exists.");

        var asset = new Asset(request.ExternalId, new Metadata(request.Title, request.Description, request.Language));
        await repo.AddAsync(asset, ct);
        await uow.SaveChangesAsync(ct);
        return asset.Id;
    }
}
