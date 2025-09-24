using Amazon.DynamoDBv2;

namespace FraudSys.WebApi.Extensions;

public static class DynamoDbExtensions
{
    public static IServiceCollection AddDynamoDbLocal(this IServiceCollection services, string serviceUrl = "http://localhost:8000")
    {
        services.AddSingleton<IAmazonDynamoDB>(sp =>
        {
            var config = new AmazonDynamoDBConfig
            {
                ServiceURL = serviceUrl,
                AuthenticationRegion = "sa-east-1"
            };

            return new AmazonDynamoDBClient("fakeMyKeyId", "fakeSecretAccessKey", config);
        });

        return services;
    }
    
    public static IServiceCollection AddDynamoDbAws(this IServiceCollection services, string region = "us-east-1")
    {
        services.AddSingleton<IAmazonDynamoDB>(sp =>
        {
            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
            };

            return new AmazonDynamoDBClient(config);
        });

        return services;
    }
}