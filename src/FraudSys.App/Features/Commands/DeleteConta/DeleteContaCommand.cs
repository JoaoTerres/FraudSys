using FraudSys.App.Result;
using MediatR;

namespace FraudSys.App.Features.Commands.DeleteConta;

public class DeleteContaCommand : IRequest<Result<bool>>
{
    public DeleteContaCommand(string documento, string agencia, string numeroDaConta)
    {
        Documento = documento;
        Agencia = agencia;
        NumeroDaConta = numeroDaConta;
    }

    public string Documento { get; }
    public string Agencia { get; }
    public string NumeroDaConta { get; }
}