using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;
using Microsoft.EntityFrameworkCore;

namespace Mediaspot.Infrastructure.Persistence;

public sealed class TitleRepository(MediaspotDbContext db) : ITitleRepository
{
    public ValueTask<Title?> GetAsync(Guid id, CancellationToken ct)
        => db.Titles.FindAsync(id, ct);

    public Task<Title?> GetByNameAsync(string name, CancellationToken ct)
        => db.Titles.FirstOrDefaultAsync(a => a.Name == name, ct);

    public Task<List<Title>> GetAllAsync(CancellationToken ct)
        => db.Titles.ToListAsync(ct);

    public async Task AddAsync(Title Title, CancellationToken ct) => await db.Titles.AddAsync(Title, ct);
}
