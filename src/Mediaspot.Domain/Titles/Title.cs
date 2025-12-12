using Mediaspot.Domain.Common;
using Mediaspot.Domain.Titles.ValueObjects;

namespace Mediaspot.Domain.Titles;

public sealed class Title : Entity
{
    // TODO : Implement and raise events if needed
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime? ReleaseDate { get; private set; }
    public TitleType? Type { get; private set; }

    private Title()
    {
        Name = string.Empty;
        Description = null;
        ReleaseDate = null;
        Type = null;
    }

    public Title(string name, string? description, DateTime? releaseDate, TitleType? type)
    {
        Name = name;
        Description = description;
        ReleaseDate = releaseDate;
        Type = type;
    }

    public void Update(string name, string? description, DateTime? releaseDate, TitleType? type)
    {
        // invariant: cannot set empty name
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name;
        Description = description;
        ReleaseDate = releaseDate;
        Type = type;
    }
}