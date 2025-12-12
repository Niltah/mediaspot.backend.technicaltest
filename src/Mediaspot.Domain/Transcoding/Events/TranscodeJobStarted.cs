using Mediaspot.Domain.Common;

namespace Mediaspot.Domain.Transcoding.Events;

public sealed record TranscodeJobStarted(Guid TrancodeJobId) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}