using FraudSys.Domain.Entities;
using MediatR;

namespace FraudSys.App.Features.Queries.GetConta;

public class GetContaQuery : IRequest<Conta?>
{
    public GetContaQuery(string documento, string agencia, string numero)
    {
        Documento = documento;
        Agencia = agencia;
        Numero = numero;
    }

    public string Documento { get; }
    public string Agencia { get; }
    public string Numero { get; }
}