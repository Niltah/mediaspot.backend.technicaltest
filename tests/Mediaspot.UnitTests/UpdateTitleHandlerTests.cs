using FluentValidation;
using Mediaspot.Application.Common;
using Mediaspot.Application.Titles.Commands.Update;
using Mediaspot.Domain.Titles;
using Mediaspot.Domain.Titles.ValueObjects;
using Moq;
using Shouldly;

namespace Mediaspot.UnitTests;

public class UpdateTitleHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_Title_And_Save()
    {
        var title = new Title("name", "description", DateTime.Now, TitleType.Series);

        var repo = new Mock<ITitleRepository>();
        var validator = new Mock<IValidator<UpdateTitleCommand>>();
        var uow = new Mock<IUnitOfWork>();

        repo.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(title);
        repo.Setup(r => r.GetByNameAsync(It.Is<string>(x => x == "name"), It.IsAny<CancellationToken>())).ReturnsAsync(title);
        repo.Setup(r => r.AddAsync(It.IsAny<Title>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new UpdateTitleHandler(repo.Object, validator.Object, uow.Object);
        var cmd = new UpdateTitleCommand(title.Id, "new-name", "description2", DateTime.Now, TitleType.Documentary);

        await handler.Handle(cmd, CancellationToken.None);

        title.Name.ShouldBe("new-name");
        title.Description.ShouldBe("description2");
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Update_Title_And_Save_With_Same_Name()
    {
        var title = new Title("name", "description", DateTime.Now, TitleType.Series);

        var repo = new Mock<ITitleRepository>();
        var validator = new Mock<IValidator<UpdateTitleCommand>>();
        var uow = new Mock<IUnitOfWork>();

        repo.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(title);
        repo.Setup(r => r.GetByNameAsync(It.Is<string>(x => x == "name"), It.IsAny<CancellationToken>())).ReturnsAsync(title);
        repo.Setup(r => r.AddAsync(It.IsAny<Title>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new UpdateTitleHandler(repo.Object, validator.Object, uow.Object);
        var cmd = new UpdateTitleCommand(title.Id, "name", "description2", DateTime.Now, TitleType.Documentary);

        await handler.Handle(cmd, CancellationToken.None);

        title.Name.ShouldBe("name");
        title.Description.ShouldBe("description2");
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Name_Exists()
    {
        var title = new Title("name", "description", DateTime.Now, TitleType.Series);
        var title2 = new Title("already-existing-name", "description", DateTime.Now, TitleType.Series);

        var repo = new Mock<ITitleRepository>();
        var validator = new Mock<IValidator<UpdateTitleCommand>>();
        var uow = new Mock<IUnitOfWork>();

        repo.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(title);
        repo.Setup(r => r.GetByNameAsync(It.Is<string>(x => x == "name"), It.IsAny<CancellationToken>())).ReturnsAsync(title);
        repo.Setup(r => r.GetByNameAsync(It.Is<string>(x => x == "already-existing-name"), It.IsAny<CancellationToken>())).ReturnsAsync(title2);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new UpdateTitleHandler(repo.Object, validator.Object, uow.Object);
        var cmd = new UpdateTitleCommand(title.Id, "already-existing-name", "description2", DateTime.Now, TitleType.Documentary);

        await Should.ThrowAsync<InvalidOperationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Id_Not_Found()
    {
        var title = new Title("name", "description", DateTime.Now, TitleType.Series);

        var repo = new Mock<ITitleRepository>();
        var validator = new Mock<IValidator<UpdateTitleCommand>>();
        var uow = new Mock<IUnitOfWork>();

        repo.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Title?)null);
        repo.Setup(r => r.GetByNameAsync(It.Is<string>(x => x == "name"), It.IsAny<CancellationToken>())).ReturnsAsync(title);
        repo.Setup(r => r.AddAsync(It.IsAny<Title>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new UpdateTitleHandler(repo.Object, validator.Object, uow.Object);
        var cmd = new UpdateTitleCommand(title.Id, "new-name", "description2", DateTime.Now, TitleType.Documentary);

        await Should.ThrowAsync<KeyNotFoundException>(() => handler.Handle(cmd, CancellationToken.None));
    }
}
