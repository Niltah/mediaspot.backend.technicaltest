using FluentValidation;
using Mediaspot.Application.Common;
using Mediaspot.Application.Titles.Commands.Create;
using Mediaspot.Domain.Titles;
using Mediaspot.Domain.Titles.ValueObjects;
using Moq;
using Shouldly;

namespace Mediaspot.UnitTests;

public class CreateTitleHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Title_And_Save()
    {
        var repo = new Mock<ITitleRepository>();
        var validator = new Mock<IValidator<CreateTitleCommand>>();
        var uow = new Mock<IUnitOfWork>();

        repo.Setup(r => r.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Title?)null);
        repo.Setup(r => r.AddAsync(It.IsAny<Title>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateTitleHandler(repo.Object, validator.Object, uow.Object);
        var cmd = new CreateTitleCommand("unique-name", "description", DateTime.Now, TitleType.Documentary);

        var id = await handler.Handle(cmd, CancellationToken.None);

        id.ShouldNotBe(Guid.Empty);
        repo.Verify(r => r.AddAsync(It.IsAny<Title>(), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Name_Exists()
    {
        var repo = new Mock<ITitleRepository>();
        var validator = new Mock<IValidator<CreateTitleCommand>>();
        var uow = new Mock<IUnitOfWork>();

        repo.Setup(r => r.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(
            new Title("unique-name", "description2", null, TitleType.Commercial));

        var handler = new CreateTitleHandler(repo.Object, validator.Object, uow.Object);
        var cmd = new CreateTitleCommand("unique-name", "description", DateTime.Now, TitleType.Documentary);

        await Should.ThrowAsync<InvalidOperationException>(() => handler.Handle(cmd, CancellationToken.None));
    }
}
