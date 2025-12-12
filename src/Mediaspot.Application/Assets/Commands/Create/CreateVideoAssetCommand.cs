namespace Mediaspot.Application.Assets.Commands.Create;

public sealed record CreateVideoAssetCommand(
    TimeSpan? Duration, KeyValuePair<int, int>? Resolution, int? FrameRate, string? Codec,
    string ExternalId, string Title, string? Description, string? Language)
    : CreateAssetCommand(ExternalId, Title, Description, Language);
