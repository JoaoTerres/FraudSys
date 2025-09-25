using FraudSys.Domain.Entities;
using MediatR;

namespace FraudSys.App.Features.Commands;

public class CreateContaCommand : IRequest<Conta>
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

    public string Documento { get; private set; }
    public string Agencia { get; private set; }
    public string NumeroDaConta { get; private set; }
    public decimal LimitePix { get; private set; }
}