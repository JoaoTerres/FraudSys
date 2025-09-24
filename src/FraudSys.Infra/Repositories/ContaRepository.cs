using Amazon.DynamoDBv2.DataModel;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;
using FraudSys.Infra.Dynamo.DataModels;
using FraudSys.Infra.Dynamo.Mappers;

namespace FraudSys.Infra.Repositories;

public class ContaRepository : IContaRepository
{
    private readonly IDynamoDBContext _context;

    public ContaRepository(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task CriarContaAsync(Conta conta)
    {
        var model = ContaMapper.ToModel(conta);
        await _context.SaveAsync(model);
    }

    public async Task<Conta?> ObterContaAsync(string documento, string agencia, string numero)
    {
        var pk = $"CONTA#{documento}";
        var sk = $"CONTA#{agencia}#{numero}";

        var model = await _context.LoadAsync<ContaDynamoModel>(pk, sk);
        return model is null ? null : ContaMapper.ToEntity(model);
    }
}