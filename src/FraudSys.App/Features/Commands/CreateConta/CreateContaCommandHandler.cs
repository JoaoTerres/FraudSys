using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;
using FraudSys.Domain.ValueObjects;
using MediatR;

namespace FraudSys.App.Features.Commands;

public class CreateContaCommandHandler : IRequestHandler<CreateContaCommand, Conta>
{
    private readonly IContaRepository _repository;

    public CreateContaCommandHandler(IContaRepository contaRepository)
    {
        _repository = contaRepository;
    }

    public async Task<Conta> Handle(CreateContaCommand request, CancellationToken cancellationToken)
    {
     
        var conta = Conta.Create(
            request.Documento, 
            request.Agencia, 
            request.Numero, 
            request.LimitePix);

        await _repository.CriarContaAsync(conta); 
        
        return conta;
    }
}