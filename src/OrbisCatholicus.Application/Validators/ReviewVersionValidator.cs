using FluentValidation;
using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Validators;

public class ReviewVersionValidator : AbstractValidator<ReviewVersionDto>
{
    public ReviewVersionValidator()
    {
        RuleFor(x => x.NewStatusId)
            .InclusiveBetween(1, 6).WithMessage("Status de revisão inválido.");
    }
}
