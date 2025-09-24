using FraudSys.Domain.Entities;

namespace FraudSys.Domain.Interfaces;

public interface IContaRepository
{
    Task CriarContaAsync(Conta conta);
    Task<Conta?> ObterContaAsync(string documento, string agencia, string numero);


}