using FluentValidation;
using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Validators;

public class CreateVersionValidator : AbstractValidator<CreateVersionDto>
{
    public CreateVersionValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Conteúdo é obrigatório.");

        RuleFor(x => x.ContentHtml)
            .NotEmpty().WithMessage("Conteúdo HTML é obrigatório.");

        RuleFor(x => x.EditSummary)
            .MaximumLength(500).WithMessage("Resumo da edição deve ter no máximo 500 caracteres.")
            .When(x => x.EditSummary != null);
    }
}
