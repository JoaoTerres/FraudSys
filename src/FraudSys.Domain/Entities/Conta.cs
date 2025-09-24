using FraudSys.Domain.ValueObjects;

namespace FraudSys.Domain.Entities;

public sealed class Conta
{
    public Cpf Documento { get; private set; }
    public string Agencia { get; private set; }
    public string Numero { get; private set; }
    public LimiteDiario LimitePix { get; private set; }

    public Conta(
        Cpf documento, 
        string agencia, 
        string numero, 
        decimal limiteDiario)
    {
        Documento = documento ?? throw new ArgumentNullException(nameof(documento));
        Agencia = string.IsNullOrWhiteSpace(agencia) ? throw new ArgumentException("Agência inválida") : agencia;
        Numero = string.IsNullOrWhiteSpace(numero) ? throw new ArgumentException("Número da conta inválido") : numero;
        LimitePix = new LimiteDiario(limiteDiario);
    }

    public bool PodeRealizarPix(decimal valor) => LimitePix.Disponivel(valor);

    public void RealizarPix(decimal valor)
    {
        LimitePix.Debitar(valor);
    }

    public void AlterarLimite(decimal novoLimite)
    {
        LimitePix = new LimiteDiario(novoLimite);
    }
}