using FluentValidation;

namespace FraudSys.App.Features.Commands.DeleteConta;

public class DeleteContaCommandValidator : AbstractValidator<DeleteContaCommand>
{
    public DeleteContaCommandValidator()
    {
        RuleFor(x => x.Documento)
            .NotEmpty().WithMessage("Documento é obrigatório")
            .Length(11).WithMessage("Documento deve ter exatamente 11 dígitos")
            .Matches(@"^\d+$").WithMessage("Documento deve conter apenas números");

        RuleFor(x => x.Agencia)
            .NotEmpty().WithMessage("Agência é obrigatória")
            .Length(4).WithMessage("Agência deve ter exatamente 4 dígitos")
            .Matches(@"^\d+$").WithMessage("Agência deve conter apenas números");

        RuleFor(x => x.NumeroDaConta)
            .NotEmpty().WithMessage("Número da conta é obrigatório")
            .MaximumLength(10).WithMessage("Número da conta deve ter no máximo 10 caracteres")
            .Matches(@"^\d+$").WithMessage("Número da conta deve conter apenas números");
    }
}