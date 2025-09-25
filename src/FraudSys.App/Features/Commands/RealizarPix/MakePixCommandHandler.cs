using FraudSys.App.Dtos;
using FraudSys.Domain.Interfaces;
using MediatR;

namespace FraudSys.App.Features.Commands.RealizarPix;

public class MakePixCommandHandler : IRequestHandler<MakePixCommand, MakePixResult>
{
    private readonly IContaRepository _repository;

    public MakePixCommandHandler(IContaRepository repository)
    {
        _repository = repository;
    }

    public async Task<MakePixResult> Handle(MakePixCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repository.ObterContaAsync(request.Documento, request.Agencia, request.NumeroDaConta);

        if (conta == null)
            return new MakePixResult { Sucesso = false, Mensagem = "Conta não encontrada." };

        if (!conta.PodeRealizarPix(request.Valor))
            return new MakePixResult { Sucesso = false, Mensagem = "Limite insuficiente." };

        conta.RealizarPix(request.Valor);

        await _repository.AtualizarContaAsync(conta);

        return new MakePixResult
        {
            Sucesso = true,
            Mensagem = "Transação aprovada.",
            LimiteRestante = conta.LimitePix.ValorMaximo - conta.LimitePix.ValorUtilizado
        };
    }
}