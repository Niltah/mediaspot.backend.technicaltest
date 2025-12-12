using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Queries.GetById;

public sealed class GetTitleByIdHandler(ITitleRepository repo) : IRequestHandler<GetTitleByIdQuery, Title>
{
    public async Task<Title> Handle(GetTitleByIdQuery request, CancellationToken ct)
    {
        return await repo.GetAsync(request.TitleId, ct) ?? throw new KeyNotFoundException("Title not found");
    }
}