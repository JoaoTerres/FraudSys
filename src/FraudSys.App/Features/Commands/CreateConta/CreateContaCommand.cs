using FraudSys.App.Dtos;
using FraudSys.App.Result;
using MediatR;

namespace FraudSys.App.Features.Commands.CreateConta;

public class CreateContaCommand : IRequest<Result<ContaResult>>
{
    public CreateContaCommand(
        string documento,
        string agencia,
        string numeroDaConta,
        decimal limitePix)
    {
        Documento = documento;
        Agencia = agencia;
        NumeroDaConta = numeroDaConta;
        LimitePix = limitePix;
    }

    public string Documento { get; }
    public string Agencia { get; }
    public string NumeroDaConta { get; }
    public decimal LimitePix { get; }
}