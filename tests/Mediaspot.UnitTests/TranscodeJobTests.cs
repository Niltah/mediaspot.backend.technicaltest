using Mediaspot.Domain.Transcoding;
using Mediaspot.Domain.Transcoding.Events;
using Shouldly;

namespace Mediaspot.UnitTests;

public class TranscodeJobTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_And_Raise_TranscodeJobCreated()
    {
        var assetId = Guid.NewGuid();
        var mediaFileId = Guid.NewGuid();
        var preset = "preset";

        var transcodeJob = new TranscodeJob(assetId, mediaFileId, preset);

        transcodeJob.AssetId.ShouldBe(assetId);
        transcodeJob.MediaFileId.ShouldBe(mediaFileId);
        transcodeJob.Preset.ShouldBe(preset);
        transcodeJob.UpdatedAt.ShouldBeNull();

        transcodeJob.Status.ShouldBe(TranscodeStatus.Pending);

        transcodeJob.DomainEvents.OfType<TranscodeJobCreated>().Any(ac => ac.TrancodeJobId == transcodeJob.Id).ShouldBeTrue();
    }

    [Fact]
    public void MarkRunning_Should_Set_Status_And_Raise_Event()
    {
        var assetId = Guid.NewGuid();
        var mediaFileId = Guid.NewGuid();
        var preset = "preset";

        var transcodeJob = new TranscodeJob(assetId, mediaFileId, preset);

        transcodeJob.MarkRunning();

        transcodeJob.AssetId.ShouldBe(assetId);
        transcodeJob.MediaFileId.ShouldBe(mediaFileId);
        transcodeJob.Preset.ShouldBe(preset);
        transcodeJob.UpdatedAt.ShouldNotBeNull();

        transcodeJob.Status.ShouldBe(TranscodeStatus.Running);

        transcodeJob.DomainEvents.OfType<TranscodeJobStarted>().Any(ac => ac.TrancodeJobId == transcodeJob.Id).ShouldBeTrue();
    }

    [Fact]
    public void MarkSucceeded_Should_Set_Status_And_Raise_Event()
    {
        var assetId = Guid.NewGuid();
        var mediaFileId = Guid.NewGuid();
        var preset = "preset";

        var transcodeJob = new TranscodeJob(assetId, mediaFileId, preset);

        transcodeJob.MarkSucceeded();

        transcodeJob.AssetId.ShouldBe(assetId);
        transcodeJob.MediaFileId.ShouldBe(mediaFileId);
        transcodeJob.Preset.ShouldBe(preset);
        transcodeJob.UpdatedAt.ShouldNotBeNull();

        transcodeJob.Status.ShouldBe(TranscodeStatus.Succeeded);

        transcodeJob.DomainEvents.OfType<TranscodeJobCompleted>()
            .Any(ac => ac.TrancodeJobId == transcodeJob.Id && ac.Success == true).ShouldBeTrue();
        transcodeJob.DomainEvents.OfType<TranscodeJobCompleted>()
            .Any(ac => ac.Success == false).ShouldBeFalse();
    }

    [Fact]
    public void MarkFailed_Should_Set_Status_And_Raise_Event()
    {
        var assetId = Guid.NewGuid();
        var mediaFileId = Guid.NewGuid();
        var preset = "preset";

        var transcodeJob = new TranscodeJob(assetId, mediaFileId, preset);

        transcodeJob.MarkFailed();

        transcodeJob.AssetId.ShouldBe(assetId);
        transcodeJob.MediaFileId.ShouldBe(mediaFileId);
        transcodeJob.Preset.ShouldBe(preset);
        transcodeJob.UpdatedAt.ShouldNotBeNull();

        transcodeJob.Status.ShouldBe(TranscodeStatus.Failed);

        transcodeJob.DomainEvents.OfType<TranscodeJobCompleted>()
            .Any(ac => ac.TrancodeJobId == transcodeJob.Id && ac.Success == false).ShouldBeTrue();
        transcodeJob.DomainEvents.OfType<TranscodeJobCompleted>()
            .Any(ac => ac.Success == true).ShouldBeFalse();
    }
}
