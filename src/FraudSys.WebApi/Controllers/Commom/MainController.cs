using FraudSys.App.Result;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.WebApi.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    protected ActionResult CustomResponse<T>(
        Result<T> result,
        bool isCreate = false,
        bool isDelete = false,
        string? getActionName = null,
        object? routeValues = null,
        string? conflictErrorKeyword = "já existe")
    {
        if (result.IsFailure)
        {
            var error = new ApiError
            {
                Code = result.Error!.Contains("não encontrada", StringComparison.InvariantCultureIgnoreCase)
                    ? "NotFound"
                    : result.Error.Contains(conflictErrorKeyword!, StringComparison.InvariantCultureIgnoreCase)
                        ? "Conflict"
                        : "ValidationError",
                Message = result.Error!,
                TraceId = HttpContext.TraceIdentifier
            };

            if (error.Code == "NotFound")
                return NotFound(error);
            if (error.Code == "Conflict")
                return Conflict(error);

            return UnprocessableEntity(error);
        }

        if (isDelete)
            return NoContent();

        if (isCreate && getActionName is not null && routeValues is not null)
            return CreatedAtAction(getActionName, routeValues, result.Value);

        if (isCreate)
            return StatusCode(201, result.Value);

        return Ok(result.Value);
    }
}