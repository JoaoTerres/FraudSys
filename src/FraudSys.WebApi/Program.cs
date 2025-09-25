using FluentValidation;
using FraudSys.App.Behaviors;
using FraudSys.App.Features.Commands;
using FraudSys.Infra;
using FraudSys.WebApi.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<CreateContaCommandValidator>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateContaCommandHandler).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services.AddDynamoDbLocal();
builder.Services.AddApiVersioningWithExplorer();
builder.Services.AddSwaggerWithVersioning();
builder.Services.AddInfraExtension();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseSwaggerWithVersioning();

await app.Services.InitializeAsync();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();