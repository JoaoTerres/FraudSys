using FraudSys.Infra.Dynamo;
using Microsoft.Extensions.DependencyInjection;

namespace FraudSys.Infra;

public static class InfraInitializer
{
    public static async Task InitializeAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<DynamoInitializer>();
        await initializer.EnsureTableExistsAsync();
    }
}