using Mediaspot.Domain.Titles.ValueObjects;
using MediatR;

namespace Mediaspot.Application.Titles.Commands.Update;

public sealed record UpdateTitleCommand(Guid TitleId, string Name, string? Description, DateTime? ReleaseDate, TitleType? Type) : IRequest;
