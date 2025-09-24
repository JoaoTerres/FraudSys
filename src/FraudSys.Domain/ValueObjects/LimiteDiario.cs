namespace FraudSys.Domain.ValueObjects;

public sealed class LimiteDiario
{
    public decimal ValorMaximo { get; }
    public decimal ValorUtilizado { get; private set; }
    public DateTime DataReferencia { get; private set; }

    public LimiteDiario(decimal valorMaximo)
    {
        if (valorMaximo <= 0)
            throw new ArgumentException("Limite diário deve ser maior que zero");

        ValorMaximo = valorMaximo;
        ValorUtilizado = 0;
        DataReferencia = DateTime.UtcNow.Date;
    }

    public bool Disponivel(decimal valor)
    {
        ResetarSeNovoDia();
        return ValorUtilizado + valor <= ValorMaximo;
    }

    public void Debitar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor deve ser positivo");

        if (!Disponivel(valor))
            throw new InvalidOperationException("Limite diário insuficiente");

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
}