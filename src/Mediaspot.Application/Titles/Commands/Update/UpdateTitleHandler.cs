using FluentValidation;
using Mediaspot.Application.Common;
using MediatR;

namespace Mediaspot.Application.Titles.Commands.Update;

public sealed class UpdateTitleHandler(ITitleRepository repo, IValidator<UpdateTitleCommand> validator, IUnitOfWork uow) : IRequestHandler<UpdateTitleCommand>
{
    public async Task Handle(UpdateTitleCommand request, CancellationToken ct)
    {
        // Validatre parameter
        validator.ValidateAndThrow(request);

        var title = await repo.GetAsync(request.TitleId, ct) ?? throw new KeyNotFoundException("Title not found");

        if (!request.Name.Equals(title.Name, StringComparison.OrdinalIgnoreCase))
        {
            // Enforce uniqueness of Name
            var existing = await repo.GetByNameAsync(request.Name, ct);
            if (existing is not null)
                throw new InvalidOperationException($"Title with Name '{request.Name}' already exists.");
        }

        title.Update(request.Name, request.Description, request.ReleaseDate, request.Type);

        await uow.SaveChangesAsync(ct);
    }
}
