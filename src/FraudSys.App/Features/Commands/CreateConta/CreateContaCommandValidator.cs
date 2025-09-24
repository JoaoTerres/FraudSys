using FluentValidation;

namespace FraudSys.App.Features.Commands;

public class CreateContaCommandValidator : AbstractValidator<CreateContaCommand>
{
    public CreateContaCommandValidator()
    {
        RuleFor(x => x.Documento)
            .NotNull().WithMessage("Documento é obrigatório")
            .NotEmpty().WithMessage("Documento (CPF) é obrigatório")
            .Length(11).WithMessage("Documento deve ter exatamente 11 dígitos")
            .Matches(@"^\d+$").WithMessage("Documento deve conter apenas números");

        RuleFor(x => x.Agencia)
            .NotEmpty().WithMessage("Agência é obrigatória")
            .Length(4).WithMessage("Agência deve ter exatamente 4 dígitos")
            .Matches(@"^\d+$").WithMessage("Agência deve conter apenas números");

        RuleFor(x => x.Numero)
            .NotEmpty().WithMessage("Número da conta é obrigatório")
            .MinimumLength(1).WithMessage("Número da conta deve ter no mínimo 1 caractere")
            .MaximumLength(10).WithMessage("Número da conta deve ter no máximo 10 caracteres")
            .Matches(@"^\d+$").WithMessage("Número da conta deve conter apenas números");

        RuleFor(x => x.LimitePix)
            .GreaterThan(0).WithMessage("O limite PIX deve ser maior que zero");
    }
}
