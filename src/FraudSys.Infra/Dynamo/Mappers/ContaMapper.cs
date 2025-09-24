using FraudSys.Domain.Entities;
using FraudSys.Domain.ValueObjects;
using FraudSys.Infra.Dynamo.DataModels;

namespace FraudSys.Infra.Dynamo.Mappers;

public static class ContaMapper
{
    public static ContaDynamoModel ToModel(Conta conta)
    {
        return new ContaDynamoModel
        {
            Documento = $"CONTA#{conta.Documento.Numero}",
            Chave = $"CONTA#{conta.Agencia}#{conta.NumeroDaConta}",
            Agencia = conta.Agencia,
            Numero = conta.NumeroDaConta,
            LimiteMaximo = conta.LimitePix.ValorMaximo,
            ValorUtilizado = conta.LimitePix.ValorUtilizado,
            DataReferencia = conta.LimitePix.DataReferencia.ToString("yyyy-MM-dd")
        };
    }

    public static Conta ToEntity(ContaDynamoModel model)
    {
        var cpf = Cpf.Restore(model.Documento.Replace("CONTA#", ""));
        var limite = LimiteDiario.Restore(
            model.LimiteMaximo,
            model.ValorUtilizado,
            DateTime.Parse(model.DataReferencia)
        );

        return Conta.Restore(cpf, model.Agencia, model.Numero, limite);
    }
}