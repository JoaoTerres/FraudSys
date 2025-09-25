using FraudSys.Domain.Entities;

namespace DomainTests;

public class ContaFixture
{
    public Conta CriarContaValida()
    {
        return Conta.Create(
            "12345678901",
            "1234",
            "56789",
            1000m);
    }
}