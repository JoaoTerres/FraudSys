namespace FraudSys.WebApi.Controllers;

public class ApiError
{
    public string Code { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string TraceId { get; set; } = default!;
}