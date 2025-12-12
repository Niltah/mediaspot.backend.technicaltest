using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Queries.GetAll;

public sealed class GetAllTitlesHandler(ITitleRepository repo) : IRequestHandler<GetAllTitlesQuery, IEnumerable<Title>>
{
    public async Task<IEnumerable<Title>> Handle(GetAllTitlesQuery request, CancellationToken ct)
    {
        return await repo.GetAllAsync(ct);
    }
}