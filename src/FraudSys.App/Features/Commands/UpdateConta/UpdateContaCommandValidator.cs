using FluentValidation;

namespace FraudSys.App.Features.Commands.UpdateConta;

public class UpdateContaCommandValidator : AbstractValidator<UpdateContaCommand>
{
    public UpdateContaCommandValidator()
    {
        RuleFor(x => x.NovoLimitePix)
            .GreaterThan(0).WithMessage("O novo limite PIX deve ser maior que zero");
    }
}