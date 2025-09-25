using FraudSys.App.Dtos;
using FraudSys.App.Result;
using MediatR;

namespace FraudSys.App.Features.Commands.UpdateConta;

public class UpdateContaCommand : IRequest<Result<ContaResult>>
{
    public UpdateContaCommand(
        string documento,
        string agencia,
        string numeroDaConta,
        decimal novoLimitePix)
    {
        Documento = documento;
        Agencia = agencia;
        NumeroDaConta = numeroDaConta;
        NovoLimitePix = novoLimitePix;
    }

    public string Documento { get; }
    public string Agencia { get; }
    public string NumeroDaConta { get; }
    public decimal NovoLimitePix { get; }
}