using System.Text.RegularExpressions;
using FraudSys.Domain.Validations;

namespace FraudSys.Domain.ValueObjects;

public sealed class Cpf : IEquatable<Cpf>
{
    public string Numero { get; }

    private Cpf(string numero)
    {
        Numero = numero;
    }

    public static Cpf Create(string numero)
    {
        var cpf = new Cpf(numero);
        cpf.Validate();
        return cpf;
    }
    public static Cpf Restore(string numero) => new Cpf(numero);

    public override string ToString() => Numero;

    public bool Equals(Cpf? other) =>
        other is not null && Numero == other.Numero;

    public override bool Equals(object? obj) => Equals(obj as Cpf);

    public override int GetHashCode() => Numero.GetHashCode();
    
    private void Validate()
    {
        AssertValidation.ValidateCpfFormat(Numero, "CPF deve conter apenas números");
        AssertValidation.ValidateIfNullOrEmpty(Numero, "CPF não pode ser vazio.");
        AssertValidation.ValidateLength(Numero, 11, 11, "CPF deve ter exatamente 11 dígitos.");

    }
}