using Asp.Versioning;
using FraudSys.App.Features.Commands.RealizarPix;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1.0)]
public class TransacoesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransacoesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("pix")]
    public async Task<ActionResult> RealizarPix([FromBody] MakePixCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Sucesso)
            return BadRequest(result);

        return Ok(result);
    }
}