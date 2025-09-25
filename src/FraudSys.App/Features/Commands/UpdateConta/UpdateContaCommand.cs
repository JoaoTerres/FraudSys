using FraudSys.Domain.Entities;
using MediatR;

namespace FraudSys.App.Features.Commands.UpdateConta;

public class UpdateContaCommand : IRequest<Conta>
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

    public string Documento { get; set; }
    public string Agencia { get; set; }
    public string NumeroDaConta { get; set; }
    public decimal NovoLimitePix { get; set; }
}