using Mediaspot.Domain.Common;
using Mediaspot.Domain.Transcoding.Events;

namespace Mediaspot.Domain.Transcoding;

public enum TranscodeStatus { Pending, Running, Succeeded, Failed }

public sealed class TranscodeJob : AggregateRoot
{
    public Guid AssetId { get; private set; }
    public Guid MediaFileId { get; private set; }
    public string Preset { get; private set; }
    public TranscodeStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private TranscodeJob()
    {
        AssetId = Guid.Empty; MediaFileId = Guid.Empty; Preset = string.Empty; CreatedAt = DateTime.Now; UpdatedAt = null;
    }

    public TranscodeJob(Guid assetId, Guid mediaFileId, string preset)
    {
        AssetId = assetId;
        MediaFileId = mediaFileId;
        Preset = preset;

        Status = TranscodeStatus.Pending;
        CreatedAt = DateTime.Now;
        UpdatedAt = null;

        Raise(new TranscodeJobCreated(Id));
    }

    public void MarkRunning()
    {
        Status = TranscodeStatus.Running;
        UpdatedAt = DateTime.Now;

        Raise(new TranscodeJobStarted(Id));
    }

    public void MarkSucceeded()
    {
        Status = TranscodeStatus.Succeeded;
        UpdatedAt = DateTime.Now;

        Raise(new TranscodeJobCompleted(Id, Success: true));
    }

    public void MarkFailed()
    {
        Status = TranscodeStatus.Failed;
        UpdatedAt = DateTime.Now;

        Raise(new TranscodeJobCompleted(Id, Success: false));
    }
}