using FraudSys.App.Dtos;
using MediatR;

namespace FraudSys.App.Features.Commands.RealizarPix;

public class RealizarPixCommand : IRequest<RealizarPixResult>
{
    public string Documento { get; set; } = string.Empty;
    public string Agencia { get; set; } = string.Empty;
    public string NumeroDaConta { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}