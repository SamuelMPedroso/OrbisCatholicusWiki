using FluentValidation;
using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Validators;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Nome de usuário é obrigatório.")
            .MinimumLength(3).WithMessage("Nome de usuário deve ter no mínimo 3 caracteres.")
            .MaximumLength(100).WithMessage("Nome de usuário deve ter no máximo 100 caracteres.")
            .Matches("^[a-zA-Z0-9._-]+$").WithMessage("Nome de usuário deve conter apenas letras, números, pontos, hifens e underscores.");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Nome de exibição é obrigatório.")
            .MaximumLength(150).WithMessage("Nome de exibição deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres.")
            .Matches("[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula.")
            .Matches("[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula.")
            .Matches("[0-9]").WithMessage("Senha deve conter pelo menos um número.");
    }
}
