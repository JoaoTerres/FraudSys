using Asp.Versioning;
using FraudSys.App.Dtos;
using FraudSys.App.Features.Commands;
using FraudSys.App.Features.Commands.UpdateConta;
using FraudSys.App.Features.Queries.GetConta;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1.0)]
public class ContaController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateContaCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(201, result);
    }

    [HttpGet("{documento}/{agencia}/{numero}")]
    public async Task<ActionResult> Get(string documento, string agencia, string numero)
    {
        var query = new GetContaQuery(documento, agencia, numero);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPatch("{documento}/{agencia}/{numero}/limite")]
    public async Task<ActionResult> AtualizarLimite(
        string documento,
        string agencia,
        string numero,
        [FromBody] UpdateContaDto body)
    {
        var command = new UpdateContaCommand(documento, agencia, numero, body.NovoLimitePix);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}