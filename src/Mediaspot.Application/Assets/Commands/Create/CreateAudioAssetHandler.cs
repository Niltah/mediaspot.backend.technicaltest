using FluentValidation;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;

namespace Mediaspot.Application.Assets.Commands.Create;

public sealed class CreateAudioAssetHandler(IAssetRepository repo, IValidator<CreateAssetCommand> validator, IUnitOfWork uow)
    : CreateAssetHandler(repo, validator, uow)
{
    public override async Task<Guid> Handle(CreateAssetCommand request, CancellationToken ct)
    {
        await ValidateData(request, ct);

        var audioRequest = request as CreateAudioAssetCommand;
        var audioAsset = new AudioAsset(audioRequest?.Duration, audioRequest?.Bitrate, audioRequest?.SampleRate, audioRequest?.Channels,
            request.ExternalId, new Metadata(request.Title, request.Description, request.Language));

        return await SaveAsset(audioAsset, ct);
    }
}
