using FraudSys.App.Dtos;
using FraudSys.Domain.Interfaces;
using MediatR;

namespace FraudSys.App.Features.Commands.RealizarPix;

public class RealizarPixCommandHandler : IRequestHandler<RealizarPixCommand, RealizarPixResult>
{
    private readonly IContaRepository _repository;

    public RealizarPixCommandHandler(IContaRepository repository)
    {
        _repository = repository;
    }

    public async Task<RealizarPixResult> Handle(RealizarPixCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repository.ObterContaAsync(request.Documento, request.Agencia, request.NumeroDaConta);

        if (conta == null)
            return new RealizarPixResult { Sucesso = false, Mensagem = "Conta não encontrada." };

        if (!conta.PodeRealizarPix(request.Valor))
            return new RealizarPixResult { Sucesso = false, Mensagem = "Limite insuficiente." };

        conta.RealizarPix(request.Valor);

        await _repository.AtualizarContaAsync(conta);

        return new RealizarPixResult
        {
            Sucesso = true,
            Mensagem = "Transação aprovada.",
            LimiteRestante = conta.LimitePix.ValorMaximo - conta.LimitePix.ValorUtilizado
        };
    }
}