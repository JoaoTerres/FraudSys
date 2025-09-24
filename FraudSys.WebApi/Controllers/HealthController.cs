using Amazon.DynamoDBv2;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1.0)]
public class HealthController : ControllerBase
{
    private readonly IAmazonDynamoDB _dynamo;

    public HealthController(IAmazonDynamoDB dynamo)
    {
        _dynamo = dynamo;
    }

    /// <summary>
    /// Checagem de saúde do DynamoDB (versão 1).
    /// </summary>
    [HttpGet("dynamo")]
    public async Task<IActionResult> CheckDynamo()
    {
        var response = await _dynamo.ListTablesAsync();
        return Ok(new
        {
            Status = "Conectado",
            Tables = response.TableNames
        });
    }
}