using FraudSys.Domain.Validations;
using FraudSys.Domain.ValueObjects;

namespace FraudSys.Domain.Entities;

public class Conta
{
    private Conta()
    {
    }

    private Conta(Cpf documento, string agencia, string numero, LimiteDiario limitePix)
    {
        Documento = documento;
        Agencia = agencia;
        NumeroDaConta = numero;
        LimitePix = limitePix;
        IsDeleted = false;
    }

    public Cpf Documento { get; }
    public string Agencia { get; }
    public string NumeroDaConta { get; }
    public LimiteDiario LimitePix { get; private set; }

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public static Conta Create(string documento, string agencia, string numero, decimal limiteDiario)
    {
        var cpf = Cpf.Create(documento);
        var limite = LimiteDiario.Create(limiteDiario);

        var conta = new Conta(cpf, agencia, numero, limite);
        conta.Validate();
        return conta;
    }

    public static Conta Restore(Cpf documento, string agencia, string numero, LimiteDiario limitePix,
        bool isDeleted = false, DateTime? deletedAt = null)
    {
        var conta = new Conta(documento, agencia, numero, limitePix);
        if (isDeleted)
        {
            conta.Delete();
            conta.DeletedAt = deletedAt;
        }

        return conta;
    }

    public void Delete()
    {
        AssertValidation.ValidateIfTrue(IsDeleted, "A conta já foi excluída.");

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }


    public bool PodeRealizarPix(decimal valor)
    {
        return LimitePix.Disponivel(valor);
    }

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
        AssertValidation.ValidateIfFalse(NumeroDaConta.All(char.IsDigit),
            "Número da conta deve conter apenas números.");
    }
}