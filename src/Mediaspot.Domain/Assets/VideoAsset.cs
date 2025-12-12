using Mediaspot.Domain.Assets.ValueObjects;

namespace Mediaspot.Domain.Assets;

public sealed class VideoAsset : Asset
{
    public TimeSpan? Duration { get; private set; }
    public KeyValuePair<int, int>? Resolution { get; private set; }
    public int? FrameRate { get; private set; }
    public string? Codec { get; private set; }

    private VideoAsset() : base() { }

    public VideoAsset(TimeSpan? duration, KeyValuePair<int, int>? resolution, int? frameRate, string? codec,
        string externalId, Metadata metadata) : base(externalId, metadata)
    {
        Duration = duration;
        Resolution = resolution;
        FrameRate = frameRate;
        Codec = codec;
    }
}