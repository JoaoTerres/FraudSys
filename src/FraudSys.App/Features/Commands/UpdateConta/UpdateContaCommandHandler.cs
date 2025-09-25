using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;
using MediatR;

namespace FraudSys.App.Features.Commands.UpdateConta;

public class UpdateContaCommandHandler : IRequestHandler<UpdateContaCommand, Conta>
{
    private readonly IContaRepository _repository;

    public UpdateContaCommandHandler(IContaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Conta> Handle(UpdateContaCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repository.ObterContaAsync(request.Documento, request.Agencia, request.NumeroDaConta);

        if (conta == null)
            throw new KeyNotFoundException("Conta não encontrada.");

        conta.AlterarLimite(request.NovoLimitePix);

        await _repository.AtualizarContaAsync(conta);

        return conta;
    }
}