using Amazon.DynamoDBv2.DataModel;
using FraudSys.Domain.Interfaces;
using FraudSys.Infra.Dynamo;
using FraudSys.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FraudSys.Infra;

public static class InfraExtensions
{
    public static IServiceCollection AddInfraExtension(this IServiceCollection services)
    {
        services.AddScoped<IContaRepository, ContaRepository>();
        services.AddScoped<IDynamoDBContext, DynamoDBContext>();

        services.AddScoped<DynamoInitializer>();


        return services;
    }
}