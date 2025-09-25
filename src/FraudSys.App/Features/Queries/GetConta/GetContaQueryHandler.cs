using FraudSys.App.Result;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;
using MediatR;

namespace FraudSys.App.Features.Queries.GetConta;

public class GetContaQueryHandler : IRequestHandler<GetContaQuery, Result<Conta>>
{
    private readonly IContaRepository _repository;

    public GetContaQueryHandler(IContaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Conta>> Handle(GetContaQuery request, CancellationToken cancellationToken)
    {
        var conta = await _repository.ObterContaAsync(request.Documento, request.Agencia, request.Numero);

        if (conta is null)
            return Result<Conta>.Failure("Conta não encontrada.");

        return Result<Conta>.Success(conta);
    }
}