namespace Mediaspot.Application.Assets.Commands.Create;

public sealed record CreateAudioAssetCommand(
    TimeSpan? Duration, int? Bitrate, int? SampleRate, string[] Channels,
    string ExternalId, string Title, string? Description, string? Language)
    : CreateAssetCommand(ExternalId, Title, Description, Language);