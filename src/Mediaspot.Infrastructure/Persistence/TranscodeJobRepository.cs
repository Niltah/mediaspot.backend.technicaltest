using Mediaspot.Application.Common;
using Mediaspot.Domain.Transcoding;
using Microsoft.EntityFrameworkCore;

namespace Mediaspot.Infrastructure.Persistence;

public sealed class TranscodeJobRepository(MediaspotDbContext db) : ITranscodeJobRepository
{
    public ValueTask<TranscodeJob?> GetAsync(Guid id, CancellationToken ct)
        => db.TranscodeJobs.FindAsync([id], cancellationToken: ct);

    public async Task AddAsync(TranscodeJob job, CancellationToken ct) => await db.TranscodeJobs.AddAsync(job, ct);

    public Task<bool> HasActiveJobsAsync(Guid assetId, CancellationToken ct)
        => db.TranscodeJobs.AnyAsync(j => j.AssetId == assetId && (j.Status == TranscodeStatus.Pending || j.Status == TranscodeStatus.Running), ct);
}
