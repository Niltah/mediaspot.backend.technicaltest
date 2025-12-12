using Mediaspot.Domain.Common;

namespace Mediaspot.Domain.Transcoding.Events;

public sealed record TranscodeJobCompleted(Guid TrancodeJobId, bool Success) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}