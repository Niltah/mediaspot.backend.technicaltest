using FluentValidation;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;

namespace Mediaspot.Application.Assets.Commands.Create;

public sealed class CreateVideoAssetHandler(IAssetRepository repo, IValidator<CreateAssetCommand> validator, IUnitOfWork uow)
    : CreateAssetHandler(repo, validator, uow)
{
    public override async Task<Guid> Handle(CreateAssetCommand request, CancellationToken ct)
    {
        await ValidateData(request, ct);

        var videoRequest = request as CreateVideoAssetCommand;
        var videoAsset = new VideoAsset(videoRequest?.Duration, videoRequest?.Resolution, videoRequest?.FrameRate, videoRequest?.Codec,
            request.ExternalId, new Metadata(request.Title, request.Description, request.Language));

        return await SaveAsset(videoAsset, ct);
    }
}
