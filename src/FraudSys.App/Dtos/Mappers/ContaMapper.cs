using FraudSys.Domain.Entities;

namespace FraudSys.App.Dtos.Mappers;

public static class ContaMapper
{
    public static ContaResult ToDto(this Conta conta)
    {
        return new ContaResult
        {
            Documento = conta.Documento.Numero,
            Agencia = conta.Agencia,
            NumeroDaConta = conta.NumeroDaConta,
            LimitePix = conta.LimitePix.ValorMaximo
        };
    }
}