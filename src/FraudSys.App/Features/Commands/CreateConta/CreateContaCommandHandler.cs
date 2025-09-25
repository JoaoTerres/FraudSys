using FraudSys.App.Dtos;
using FraudSys.App.Dtos.Mappers;
using FraudSys.App.Features.Commands.CreateConta;
using FraudSys.App.Result;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Exceptions;
using FraudSys.Domain.Interfaces;
using MediatR;

namespace FraudSys.App.Features.Commands;

public class CreateContaCommandHandler : IRequestHandler<CreateContaCommand, Result<ContaResult>>
{
    private readonly IContaRepository _repository;

    public CreateContaCommandHandler(IContaRepository contaRepository)
    {
        _repository = contaRepository;
    }

    public async Task<Result<ContaResult>> Handle(CreateContaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existente =
                await _repository.ObterContaAsync(request.Documento, request.Agencia, request.NumeroDaConta);

            if (existente is not null)
                return Result<ContaResult>.Failure("Já existe uma conta com esses dados.");

            var conta = Conta.Create(
                request.Documento,
                request.Agencia,
                request.NumeroDaConta,
                request.LimitePix);

            await _repository.CriarContaAsync(conta);

            return Result<ContaResult>.Success(conta.ToDto());
        }
        catch (DomainException ex)
        {
            return Result<ContaResult>.Failure(ex.Message);
        }
        catch (Exception)
        {
            return Result<ContaResult>.Failure("Erro inesperado ao criar conta.");
        }
    }
}