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
            DataReferencia = conta.LimitePix.DataReferencia.ToString("yyyy-MM-dd"),

            // 🔹 Soft Delete
            IsDeleted = conta.IsDeleted,
            DeletedAt = conta.DeletedAt?.ToString("O") // ISO 8601 para consistência
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

        var conta = Conta.Restore(cpf, model.Agencia, model.Numero, limite);

        if (model.IsDeleted)
        {
            typeof(Conta).GetProperty("IsDeleted")!.SetValue(conta, model.IsDeleted);
            if (!string.IsNullOrEmpty(model.DeletedAt))
                typeof(Conta).GetProperty("DeletedAt")!.SetValue(conta, DateTime.Parse(model.DeletedAt));
        }

        return conta;
    }
}