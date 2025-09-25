using Asp.Versioning;
using FraudSys.App.Dtos;
using FraudSys.App.Features.Commands.CreateConta;
using FraudSys.App.Features.Commands.DeleteConta;
using FraudSys.App.Features.Commands.UpdateConta;
using FraudSys.App.Features.Queries.GetConta;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1.0)]
public class ContaController : MainController
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

        return CustomResponse(
            result,
            true,
            getActionName: nameof(Get),
            routeValues: new
            {
                documento = result.Value?.Documento,
                agencia = result.Value?.Agencia,
                numero = result.Value?.NumeroDaConta
            }
        );
    }

    [HttpGet("{documento}/{agencia}/{numero}")]
    public async Task<ActionResult> Get(string documento, string agencia, string numero)
    {
        var query = new GetContaQuery(documento, agencia, numero);
        var result = await _mediator.Send(query);
        return CustomResponse(result);
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
        return CustomResponse(result);
    }

    [HttpDelete("{documento}/{agencia}/{numero}")]
    public async Task<ActionResult> Delete(string documento, string agencia, string numero)
    {
        var command = new DeleteContaCommand(documento, agencia, numero);
        var result = await _mediator.Send(command);

        return CustomResponse(result, isDelete: true);
    }
}