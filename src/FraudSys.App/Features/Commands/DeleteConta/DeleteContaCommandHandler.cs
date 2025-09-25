using FraudSys.App.Result;
using FraudSys.Domain.Exceptions;
using FraudSys.Domain.Interfaces;
using MediatR;

namespace FraudSys.App.Features.Commands.DeleteConta;

public class DeleteContaCommandHandler : IRequestHandler<DeleteContaCommand, Result<bool>>
{
    private readonly IContaRepository _repository;

    public DeleteContaCommandHandler(IContaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeleteContaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var conta = await _repository.ObterContaAsync(request.Documento, request.Agencia, request.NumeroDaConta);

            if (conta is null)
                return Result<bool>.Failure("Conta não encontrada.");

            conta.Delete();

            await _repository.AtualizarContaAsync(conta);

            return Result<bool>.Success(true);
        }
        catch (DomainException ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
        catch (Exception)
        {
            return Result<bool>.Failure("Erro inesperado ao excluir conta.");
        }
    }
}