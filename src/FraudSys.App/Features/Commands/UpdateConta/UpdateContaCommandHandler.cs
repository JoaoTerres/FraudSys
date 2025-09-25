using FraudSys.App.Dtos;
using FraudSys.App.Dtos.Mappers;
using FraudSys.App.Result;
using FraudSys.Domain.Exceptions;
using FraudSys.Domain.Interfaces;
using MediatR;

namespace FraudSys.App.Features.Commands.UpdateConta;

public class UpdateContaCommandHandler : IRequestHandler<UpdateContaCommand, Result<ContaResult>>
{
    private readonly IContaRepository _repository;

    public UpdateContaCommandHandler(IContaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ContaResult>> Handle(UpdateContaCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repository.ObterContaAsync(request.Documento, request.Agencia, request.NumeroDaConta);

        if (conta is null)
            return Result<ContaResult>.Failure("Conta não encontrada.");

        try
        {
            conta.AlterarLimite(request.NovoLimitePix);
        }
        catch (DomainException ex)
        {
            return Result<ContaResult>.Failure(ex.Message);
        }

        await _repository.AtualizarContaAsync(conta);

        return Result<ContaResult>.Success(conta.ToDto());
    }
}