using FraudSys.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDynamoDbLocal();
builder.Services.AddApiVersioningWithExplorer();
builder.Services.AddSwaggerWithVersioning();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithVersioning();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();