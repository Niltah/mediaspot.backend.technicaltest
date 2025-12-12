using FluentValidation;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using MediatR;

namespace Mediaspot.Application.Assets.Commands.Create;

public abstract class CreateAssetHandler(IAssetRepository repo, IValidator<CreateAssetCommand> validator, IUnitOfWork uow)
    : IRequestHandler<CreateAssetCommand, Guid>
{

    public abstract Task<Guid> Handle(CreateAssetCommand request, CancellationToken ct);

    protected async Task ValidateData(CreateAssetCommand request, CancellationToken ct)
    {
        // Validate parameter
        validator.ValidateAndThrow(request);

        // Enforce uniqueness of ExternalId
        var existing = await repo.GetByExternalIdAsync(request.ExternalId, ct);
        if (existing is not null)
            throw new InvalidOperationException($"Asset with ExternalId '{request.ExternalId}' already exists.");
    }

    protected async Task<Guid> SaveAsset(Asset asset, CancellationToken ct)
    {
        await repo.AddAsync(asset, ct);
        await uow.SaveChangesAsync(ct);
        return asset.Id;
    }
}
