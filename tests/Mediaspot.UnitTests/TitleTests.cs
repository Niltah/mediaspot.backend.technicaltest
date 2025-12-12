using Mediaspot.Domain.Titles;
using Mediaspot.Domain.Titles.ValueObjects;
using Shouldly;

namespace Mediaspot.UnitTests;

public class TitleTests
{
    [Fact]
    public void Constructor_Should_Set_Properties()
    {
        var name = "name";
        var description = "descripion";
        var releaseDate = DateTime.Now;
        var type = TitleType.Documentary;

        var title = new Title(name, description, releaseDate, type);

        title.Name.ShouldBe(name);
        title.Description.ShouldBe(description);
        title.ReleaseDate.ShouldBe(releaseDate);
        title.Type.ShouldBe(type);
    }

    [Fact]
    public void Update_Should_Set_Properties()
    {
        var name = "name";
        var description = "descripion";
        var releaseDate = DateTime.Now;
        var type = TitleType.Documentary;

        var title = new Title(name, description, releaseDate, type);

        var newName = "new-name";
        var newDescription = "new-descripion";
        var newReleaseDate = DateTime.Today;
        var newType = TitleType.Movie;

        title.Update(newName, newDescription, newReleaseDate, newType);

        title.Name.ShouldBe(newName);
        title.Description.ShouldBe(newDescription);
        title.ReleaseDate.ShouldBe(newReleaseDate);
        title.Type.ShouldBe(newType);
    }

    [Fact]
    public void UpdateMetadata_Should_Throw_If_Name_Empty()
    {
        var name = "name";
        var description = "descripion";
        var releaseDate = DateTime.Now;
        var type = TitleType.Documentary;

        var title = new Title(name, description, releaseDate, type);

        Should.Throw<ArgumentException>(() => title.Update(string.Empty, description, releaseDate, type));
    }
}
