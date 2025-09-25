using FraudSys.Domain.Validations;

namespace FraudSys.Domain.ValueObjects;

public sealed class Cpf : IEquatable<Cpf>
{
    private Cpf(string numero)
    {
        Numero = numero;
    }

    public string Numero { get; }

    public bool Equals(Cpf? other)
    {
        return other is not null && Numero == other.Numero;
    }

    public static Cpf Create(string numero)
    {
        var cpf = new Cpf(numero);
        cpf.Validate();
        return cpf;
    }

    public static Cpf Restore(string numero)
    {
        return new Cpf(numero);
    }

    public override string ToString()
    {
        return Numero;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Cpf);
    }

    public override int GetHashCode()
    {
        return Numero.GetHashCode();
    }

    private void Validate()
    {
        AssertValidation.ValidateIfNullOrEmpty(Numero, "CPF não pode ser vazio.");
        AssertValidation.ValidateLength(Numero, 11, 11, "CPF deve ter exatamente 11 dígitos.");
        AssertValidation.ValidateCpfFormat(Numero, "CPF deve conter apenas números");
    }
}