using Mediaspot.Application.Common;
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
        var repo = new Mock<ITranscodeJobRepository>();
        var uow = new Mock<IUnitOfWork>();

        var job = new TranscodeJob(Guid.NewGuid(), Guid.NewGuid(), "preset");

        repo.Setup(r => r.GetAsync(It.Is<Guid>(x => x == job.Id), It.IsAny<CancellationToken>())).ReturnsAsync(job);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var worker = new TranscodeJobWorker(repo.Object, uow.Object);

        await worker.ProcessJobAsync(job.Id, CancellationToken.None);

        job.Status.ShouldBe(TranscodeStatus.Succeeded);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
