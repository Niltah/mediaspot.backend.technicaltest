using FluentValidation;

namespace Mediaspot.Application.Titles.Commands.Create;

public sealed class CreateTitleValidator : AbstractValidator<CreateTitleCommand>
{
    public CreateTitleValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
