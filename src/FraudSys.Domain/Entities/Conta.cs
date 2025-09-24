using FraudSys.Domain.Validations;
using FraudSys.Domain.ValueObjects;

namespace FraudSys.Domain.Entities;

public class Conta
{
    public Cpf Documento { get; private set; }
    public string Agencia { get; private set; }
    public string NumeroDaConta { get; private set; }
    public LimiteDiario LimitePix { get; private set; }

    private Conta() { }

    private Conta(Cpf documento, string agencia, string numero, LimiteDiario limitePix)
    {
        Documento = documento;
        Agencia = agencia;
        NumeroDaConta = numero;
        LimitePix = limitePix;
    }

    public static Conta Create(string documento, string agencia, string numero, decimal limiteDiario)
    {
        var cpf = Cpf.Create(documento);
        var limite = LimiteDiario.Create(limiteDiario);

        var conta = new Conta(cpf, agencia, numero, limite);
        conta.Validate(); 
        return conta;
    }

    public static Conta Restore(Cpf documento, string agencia, string numero, LimiteDiario limitePix)
        => new Conta(documento, agencia, numero, limitePix);

    public bool PodeRealizarPix(decimal valor) => LimitePix.Disponivel(valor);

    public void RealizarPix(decimal valor)
    {
        LimitePix.Debitar(valor);
    }

    public void AlterarLimite(decimal novoLimite)
    {
        LimitePix = LimiteDiario.Create(novoLimite);
    }

    private void Validate()
    {
        AssertValidation.ValidateIfNull(Documento, "CPF é obrigatório.");
        AssertValidation.ValidateIfNullOrEmpty(Agencia, "Agência é obrigatória.");
        AssertValidation.ValidateLength(Agencia, 4, 4, "Agência deve ter exatamente 4 caracteres.");
        AssertValidation.ValidateIfFalse(Agencia.All(char.IsDigit), "Agência deve conter apenas números.");
        AssertValidation.ValidateIfNullOrEmpty(NumeroDaConta, "Número da conta é obrigatório.");
        AssertValidation.ValidateLength(NumeroDaConta, 1, 10, "Número da conta deve ter no máximo 10 caracteres.");
        AssertValidation.ValidateIfFalse(NumeroDaConta.All(char.IsDigit), "Número da conta deve conter apenas números.");
    }
}