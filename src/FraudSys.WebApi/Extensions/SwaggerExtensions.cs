using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace FraudSys.WebApi.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithVersioning(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FraudSys API",
                Version = "v1",
                Description = "API para gestão de limites de PIX"
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerWithVersioning(this IApplicationBuilder app)
    {
        var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant()
                );
            }
        });

        return app;
    }
}