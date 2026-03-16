using FluentValidation;
using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Validators;

public class CreateArticleValidator : AbstractValidator<CreateArticleDto>
{
    public CreateArticleValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MaximumLength(255).WithMessage("Título deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Conteúdo é obrigatório.");

        RuleFor(x => x.ContentHtml)
            .NotEmpty().WithMessage("Conteúdo HTML é obrigatório.");

        RuleFor(x => x.Summary)
            .MaximumLength(1000).WithMessage("Resumo deve ter no máximo 1000 caracteres.")
            .When(x => x.Summary != null);
    }
}
