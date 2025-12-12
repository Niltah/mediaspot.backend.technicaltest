using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Queries.GetAll;

public sealed record GetAllTitlesQuery() : IRequest<IEnumerable<Title>>;
