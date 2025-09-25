using FluentValidation;

namespace FraudSys.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Erro de validação: {Erros}", ex.Errors);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new
            {
                errors = ex.Errors.Select(e => new
                {
                    code = "VALIDATION_ERROR",
                    message = e.ErrorMessage,
                    traceId = context.TraceIdentifier
                })
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}