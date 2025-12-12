using Mediaspot.Domain.Assets.ValueObjects;

namespace Mediaspot.Domain.Assets;

public sealed class AudioAsset : Asset
{
    private readonly List<string> _channels = [];

    public TimeSpan? Duration { get; private set; }
    public int? Bitrate { get; private set; }
    public int? SampleRate { get; private set; }
    public IReadOnlyCollection<string> Channels => _channels.AsReadOnly();

    private AudioAsset() : base() { }

    public AudioAsset(TimeSpan? duration, int? bitrate, int? sampleRate, string[]? channels,
        string externalId, Metadata metadata) : base(externalId, metadata)
    {
        Duration = duration;
        Bitrate = bitrate;
        SampleRate = sampleRate;

        if (channels != null)
        {
            _channels.AddRange(channels);
        }
    }
}