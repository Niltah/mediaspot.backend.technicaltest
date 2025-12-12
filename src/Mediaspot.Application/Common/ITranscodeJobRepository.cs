using Mediaspot.Domain.Transcoding;

namespace Mediaspot.Application.Common;

public interface ITranscodeJobRepository
{
    ValueTask<TranscodeJob?> GetAsync(Guid id, CancellationToken ct);
    Task AddAsync(TranscodeJob job, CancellationToken ct);
    Task<bool> HasActiveJobsAsync(Guid assetId, CancellationToken ct);
}
