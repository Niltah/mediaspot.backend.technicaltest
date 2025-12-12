using FluentValidation;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Commands.Create;

public sealed class CreateTitleHandler(ITitleRepository repo, IValidator<CreateTitleCommand> validator, IUnitOfWork uow)
    : IRequestHandler<CreateTitleCommand, Guid>
{
    public async Task<Guid> Handle(CreateTitleCommand request, CancellationToken ct)
    {
        // Validatre parameter
        validator.ValidateAndThrow(request);

        // Enforce uniqueness of Name
        var existing = await repo.GetByNameAsync(request.Name, ct);
        if (existing is not null)
            throw new InvalidOperationException($"Title with Name '{request.Name}' already exists.");

        var Title = new Title(request.Name, request.Description, request.ReleaseDate, request.Type);

        await repo.AddAsync(Title, ct);
        await uow.SaveChangesAsync(ct);

        return Title.Id;
    }
}
