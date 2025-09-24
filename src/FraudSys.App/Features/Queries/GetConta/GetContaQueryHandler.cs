using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;
using MediatR;

namespace FraudSys.App.Features.Queries.GetConta;

public class GetContaQueryHandler : IRequestHandler<GetContaQuery, Conta?>
{
    private readonly IContaRepository _repository;

    public GetContaQueryHandler(IContaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Conta?> Handle(GetContaQuery request, CancellationToken cancellationToken)
    {
        return await _repository.ObterContaAsync(request.Documento, request.Agencia, request.Numero);
    }
}