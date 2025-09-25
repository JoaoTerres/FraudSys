namespace FraudSys.App.Dtos;

public class ContaResult
{
    public required string Documento { get; set; }
    public required string Agencia { get; set; }
    public required string NumeroDaConta { get; set; }
    public required decimal LimitePix { get; set; }
}