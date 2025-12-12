using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;
using Mediaspot.Domain.Transcoding;
using Mediaspot.Worker.Transcoding;
using Moq;
using Shouldly;

namespace Mediaspot.UnitTests;

public class TranscodeJobWorkerTests
{
    [Fact]
    public async Task ProcessJob_Succes_MarksSucceeded()
    {
        var transcodeRepo = new Mock<ITranscodeJobRepository>();
        var assetRepo = new Mock<IAssetRepository>();
        var uow = new Mock<IUnitOfWork>();

        var job = new TranscodeJob(Guid.NewGuid(), Guid.NewGuid(), "preset");
        var asset = new VideoAsset(null, null, null, null, "external-id", new Metadata("t", null, null));

        transcodeRepo.Setup(r => r.GetAsync(It.Is<Guid>(x => x == job.Id), It.IsAny<CancellationToken>())).ReturnsAsync(job);
        assetRepo.Setup(r => r.GetAsync(It.Is<Guid>(x => x == job.AssetId), It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var worker = new TranscodeJobWorker(transcodeRepo.Object, assetRepo.Object, uow.Object);

        await worker.ProcessJobAsync(job.Id, CancellationToken.None);

        job.Status.ShouldBe(TranscodeStatus.Succeeded);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
