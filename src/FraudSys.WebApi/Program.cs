using FraudSys.Infra;
using FraudSys.Infra.Dynamo;
using FraudSys.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDynamoDbLocal();
builder.Services.AddApiVersioningWithExplorer();
builder.Services.AddSwaggerWithVersioning();
builder.Services.AddInfraExtension();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithVersioning();
}

await app.Services.InitializeAsync();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();