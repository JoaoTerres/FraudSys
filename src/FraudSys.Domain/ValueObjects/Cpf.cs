namespace FraudSys.Domain.ValueObjects;

public sealed class Cpf : IEquatable<Cpf>
{
    public string Numero { get; }

    public Cpf(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("CPF não pode ser vazio");

        var somenteDigitos = new string(numero.Where(char.IsDigit).ToArray());

        if (somenteDigitos.Length != 11 || !ValidarCpf(somenteDigitos))
            throw new ArgumentException("CPF inválido");

        Numero = somenteDigitos;
    }

    public override string ToString() => Numero;

    public bool Equals(Cpf? other) =>
        other is not null && Numero == other.Numero;

    public override bool Equals(object? obj) => Equals(obj as Cpf);

    public override int GetHashCode() => Numero.GetHashCode();

    private bool ValidarCpf(string numero)
    {
        return numero.Length == 11;
    }
}
