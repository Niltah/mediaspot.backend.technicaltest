using FluentValidation;

namespace Mediaspot.Application.Titles.Commands.Update;

public sealed class UpdateTitleValidator : AbstractValidator<UpdateTitleCommand>
{
    public UpdateTitleValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
