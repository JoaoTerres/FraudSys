using FluentAssertions;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Exceptions;
using FraudSys.Domain.ValueObjects;

namespace DomainTests;

public class ContaTests : IClassFixture<ContaFixture>
{
    private readonly ContaFixture _fixture;

    public ContaTests(ContaFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Create_ContaValida_DeveCriarInstancia()
    {
        // Arrange
        var documento = "12345678901";
        var agencia = "1234";
        var numero = "56789";
        var limite = 1000m;

        // Act
        var conta = Conta.Create(documento, agencia, numero, limite);

        // Assert
        conta.Should().NotBeNull();
        conta.Documento.Numero.Should().Be(documento);
        conta.Agencia.Should().Be(agencia);
        conta.NumeroDaConta.Should().Be(numero);
        conta.LimitePix.ValorMaximo.Should().Be(limite);
    }

    [Theory]
    [InlineData(null, "1234", "56789", 1000)]
    [InlineData("123", "1234", "56789", 1000)]
    [InlineData("12345678901", null, "56789", 1000)]
    [InlineData("12345678901", "12", "56789", 1000)]
    [InlineData("12345678901", "1234", null, 1000)]
    [InlineData("12345678901", "1234", "ABCDE", 1000)]
    public void Create_ContaInvalida_DeveLancarExcecao(
        string documento, string agencia, string numero, decimal limite)
    {
        // Act
        Action act = () => Conta.Create(documento!, agencia!, numero!, limite);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*");
    }

    [Fact]
    public void PodeRealizarPix_ValorDentroDoLimite_DeveRetornarTrue()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();
        var valor = 200m;

        // Act
        var resultado = conta.PodeRealizarPix(valor);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public void PodeRealizarPix_ValorAcimaDoLimite_DeveRetornarFalse()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();
        var valor = 2000m;

        // Act
        var resultado = conta.PodeRealizarPix(valor);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void RealizarPix_ValorValido_DeveDebitarLimite()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();
        var valor = 100m;

        // Act
        conta.RealizarPix(valor);

        // Assert
        conta.LimitePix.ValorUtilizado.Should().Be(valor);
    }

    [Fact]
    public void RealizarPix_ValorAcimaDoLimite_DeveLancarExcecao()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();
        var valor = 2000m;

        // Act
        var act = () => conta.RealizarPix(valor);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Limite diário insuficiente.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Create_LimiteInvalido_DeveLancarExcecao(decimal limiteInvalido)
    {
        // Act
        Action act = () => Conta.Create("12345678901", "1234", "56789", limiteInvalido);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Limite diário deve ser maior que zero.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void AlterarLimite_ValorInvalido_DeveLancarExcecao(decimal limiteInvalido)
    {
        // Arrange
        var conta = _fixture.CriarContaValida();

        // Act
        var act = () => conta.AlterarLimite(limiteInvalido);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Limite diário deve ser maior que zero.");
    }
    
    [Fact]
    public void Restore_ComParametrosValidos_DeveCriarConta()
    {
        // Arrange
        var cpf = Cpf.Create("12345678901");
        var limite = LimiteDiario.Create(1000m);

        // Act
        var conta = Conta.Restore(cpf, "1234", "56789", limite);

        // Assert
        conta.Should().NotBeNull();
        conta.Documento.Should().Be(cpf);
        conta.Agencia.Should().Be("1234");
        conta.NumeroDaConta.Should().Be("56789");
        conta.LimitePix.Should().Be(limite);
    }
    
    [Fact]
    public void Create_CpfNulo_DeveLancarExcecao()
    {
        // Act
        Action act = () => Conta.Create(null!, "1234", "56789", 1000m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("CPF não pode ser vazio.");
    }
    
    [Fact]
    public void Restore_ComParametrosInvalidos_DeveCriarMesmoAssim()
    {
        // Arrange
        var cpf = Cpf.Restore("abc");
        var limite = LimiteDiario.Restore(0.01m, 0m, DateTime.UtcNow.Date);

        // Act
        var conta = Conta.Restore(cpf, "", "ABC", limite);

        // Assert
        conta.Documento.Numero.Should().Be("abc");
        conta.Agencia.Should().Be("");
        conta.NumeroDaConta.Should().Be("ABC");
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void RealizarPix_ValorInvalido_DeveLancarExcecao(decimal valorInvalido)
    {
        // Arrange
        var conta = _fixture.CriarContaValida();

        // Act
        Action act = () => conta.RealizarPix(valorInvalido);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Valor*");
    }
    
    [Fact]
    public void PodeRealizarPix_AposAlterarLimite_DeveRefletirNovoLimite()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();

        // Act
        conta.AlterarLimite(50m);
        var resultado = conta.PodeRealizarPix(60m);

        // Assert
        resultado.Should().BeFalse();
    }
    
    [Fact]
    public void RealizarPix_MultiplasVezes_DeveAcumularNoLimite()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();

        // Act
        conta.RealizarPix(200m);
        conta.RealizarPix(300m);

        // Assert
        conta.LimitePix.ValorUtilizado.Should().Be(500m);
        conta.PodeRealizarPix(600m).Should().BeFalse();
    }
    
    [Fact]
    public void AlterarLimite_ComValorValido_DeveTrocarLimiteInternamente()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();
        var limiteAntigo = conta.LimitePix.ValorMaximo;
        var novoLimite = limiteAntigo + 500m;

        // Act
        conta.AlterarLimite(novoLimite);

        // Assert
        conta.LimitePix.ValorMaximo.Should().Be(novoLimite);
        conta.LimitePix.ValorMaximo.Should().NotBe(limiteAntigo);
    }
    
}