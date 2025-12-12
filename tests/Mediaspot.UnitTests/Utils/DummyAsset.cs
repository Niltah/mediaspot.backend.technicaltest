using FluentValidation;
using Mediaspot.Application.Assets.Commands.Create;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;

namespace Mediaspot.UnitTests.Utils;

// TODO Rewrite unit tests to test VideoAsset and AudioAsset class instead of these dummy classes

public sealed class DummyAsset(string a, Metadata b) : Asset(a, b) { }

public sealed record class DummyCreateAssetCommand(string A, string B, string? C, string? D)
    : CreateAssetCommand(A, B, C, D);

public sealed class DummyCreateAssetHandler(IAssetRepository a, IValidator<CreateAssetCommand> b, IUnitOfWork c)
    : CreateAssetHandler(a, b, c)
{
    public override async Task<Guid> Handle(CreateAssetCommand request, CancellationToken ct)
    {
        await ValidateData(request, ct);

        var asset = new DummyAsset(request.ExternalId, new Metadata(request.Title, request.Description, request.Language));

        return await SaveAsset(asset, ct);
    }
}