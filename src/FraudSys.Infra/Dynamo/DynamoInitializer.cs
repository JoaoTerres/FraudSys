using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace FraudSys.Infra.Dynamo;

public class DynamoInitializer
{
    private readonly IAmazonDynamoDB _dynamo;

    public DynamoInitializer(IAmazonDynamoDB dynamo)
    {
        _dynamo = dynamo;
    }

    public async Task EnsureTableExistsAsync()
    {
        const string tableName = "Contas";
        var tables = await _dynamo.ListTablesAsync();

        if (!tables.TableNames.Contains(tableName))
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new("PK", ScalarAttributeType.S),
                    new("SK", ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new("PK", KeyType.HASH),
                    new("SK", KeyType.RANGE)
                },
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            };

            await _dynamo.CreateTableAsync(request);
        }
    }
}