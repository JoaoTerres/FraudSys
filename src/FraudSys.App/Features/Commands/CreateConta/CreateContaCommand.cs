using FraudSys.Domain.Entities;
using FraudSys.Domain.ValueObjects;
using MediatR;

namespace FraudSys.App.Features.Commands;

public class CreateContaCommand : IRequest<Conta>
{
    public string Documento { get; private set; }
    public string Agencia { get; private set; }
    public string Numero { get; private set; }
    public decimal LimitePix { get; private set; }

    public CreateContaCommand(
        string documento, 
        string agencia, 
        string numero, 
        decimal limitePix)
    {
        Documento = documento;
        Agencia = agencia;
        Numero = numero;
        LimitePix = limitePix;
    }
}