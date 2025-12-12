using Mediaspot.Application.Assets.Commands.Archive;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;
using Mediaspot.UnitTests.Utils;
using Moq;
using Shouldly;

namespace Mediaspot.UnitTests;

public class ArchiveAssetHandlerTests
{
    [Fact]
    public async Task Handle_Should_Archive_Asset_And_Save()
    {
        var asset = new DummyAsset("ext", new Metadata("t", null, null));
        var repo = new Mock<IAssetRepository>();
        var jobs = new Mock<ITranscodeJobRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        jobs.Setup(j => j.HasActiveJobsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var handler = new ArchiveAssetHandler(repo.Object, jobs.Object, uow.Object);
        var cmd = new ArchiveAssetCommand(asset.Id);

        await handler.Handle(cmd, CancellationToken.None);

        asset.Archived.ShouldBeTrue();
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_If_Asset_Not_Found()
    {
        var repo = new Mock<IAssetRepository>();
        var jobs = new Mock<ITranscodeJobRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Asset?)null);
        var handler = new ArchiveAssetHandler(repo.Object, jobs.Object, uow.Object);
        var cmd = new ArchiveAssetCommand(Guid.NewGuid());

        await Should.ThrowAsync<KeyNotFoundException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Throw_If_ActiveJobs()
    {
        var asset = new DummyAsset("ext", new Metadata("t", null, null));
        var repo = new Mock<IAssetRepository>();
        var jobs = new Mock<ITranscodeJobRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        jobs.Setup(j => j.HasActiveJobsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var handler = new ArchiveAssetHandler(repo.Object, jobs.Object, uow.Object);
        var cmd = new ArchiveAssetCommand(asset.Id);

        await Should.ThrowAsync<InvalidOperationException>(() => handler.Handle(cmd, CancellationToken.None));
    }
}
