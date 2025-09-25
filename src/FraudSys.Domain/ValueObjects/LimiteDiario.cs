using FraudSys.Domain.Validations;

namespace FraudSys.Domain.ValueObjects;

public sealed class LimiteDiario
{
    private LimiteDiario(decimal valorMaximo)
    {
        ValorMaximo = valorMaximo;
        ValorUtilizado = 0;
        DataReferencia = DateTime.UtcNow.Date;
    }

    public decimal ValorMaximo { get; }
    public decimal ValorUtilizado { get; private set; }
    public DateTime DataReferencia { get; private set; }

    public static LimiteDiario Create(decimal valorMaximo)
    {
        var limite = new LimiteDiario(valorMaximo);
        limite.Validate();
        return limite;
    }

    public static LimiteDiario Restore(decimal max, decimal usado, DateTime refDate)
    {
        return new LimiteDiario(max) { ValorUtilizado = usado, DataReferencia = refDate };
    }


    public bool Disponivel(decimal valor)
    {
        ResetarSeNovoDia();
        return ValorUtilizado + valor <= ValorMaximo;
    }

    public void Debitar(decimal valor)
    {
        AssertValidation.ValidateIfLowerThen((long)valor, 1, "Valor deve ser positivo.");

        if (!Disponivel(valor))
            throw new InvalidOperationException("Limite diário insuficiente.");

        ValorUtilizado += valor;
    }

    private void ResetarSeNovoDia()
    {
        if (DataReferencia < DateTime.UtcNow.Date)
        {
            ValorUtilizado = 0;
            DataReferencia = DateTime.UtcNow.Date;
        }
    }

    private void Validate()
    {
        AssertValidation.ValidateIfLowerThen((long)ValorMaximo, 1, "Limite diário deve ser maior que zero.");
    }
}