using Amazon.DynamoDBv2.DataModel;

namespace FraudSys.Infra.Dynamo.DataModels;

[DynamoDBTable("Contas")]
public class ContaDynamoModel
{
    [DynamoDBHashKey("PK")] public string Documento { get; set; } = default!;

    [DynamoDBRangeKey("SK")] public string Chave { get; set; } = default!;

    [DynamoDBProperty] public string Agencia { get; set; } = default!;

    [DynamoDBProperty] public string Numero { get; set; } = default!;

    [DynamoDBProperty] public decimal LimiteMaximo { get; set; }

    [DynamoDBProperty] public decimal ValorUtilizado { get; set; }

    [DynamoDBProperty] public string DataReferencia { get; set; } = default!;
}