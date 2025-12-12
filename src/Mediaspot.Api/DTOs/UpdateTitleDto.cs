using Mediaspot.Domain.Titles.ValueObjects;

namespace Mediaspot.Api.DTOs;

public sealed record UpdateTitleDto(string Name, string? Description, DateTime? ReleaseDate, TitleType? Type);
