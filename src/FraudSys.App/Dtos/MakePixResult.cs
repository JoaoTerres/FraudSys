namespace FraudSys.App.Dtos;

public class MakePixResult
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public decimal? LimiteRestante { get; set; }
}