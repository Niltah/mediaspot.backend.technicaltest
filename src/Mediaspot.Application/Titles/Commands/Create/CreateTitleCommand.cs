using Mediaspot.Domain.Titles.ValueObjects;
using MediatR;

namespace Mediaspot.Application.Titles.Commands.Create;

public sealed record CreateTitleCommand(string Name, string? Description, DateTime? ReleaseDate, TitleType? Type) : IRequest<Guid>;