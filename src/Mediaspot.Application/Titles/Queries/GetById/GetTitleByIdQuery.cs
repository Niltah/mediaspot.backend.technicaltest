using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Queries.GetById;

public sealed record GetTitleByIdQuery(Guid TitleId) : IRequest<Title>;
