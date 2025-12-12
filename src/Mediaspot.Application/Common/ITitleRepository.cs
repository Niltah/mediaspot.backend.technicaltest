using Mediaspot.Domain.Titles;

namespace Mediaspot.Application.Common;

public interface ITitleRepository
{
    ValueTask<Title?> GetAsync(Guid id, CancellationToken ct);

    Task<Title?> GetByNameAsync(string name, CancellationToken ct);

    Task<List<Title>> GetAllAsync(CancellationToken ct);

    Task AddAsync(Title asset, CancellationToken ct);
}