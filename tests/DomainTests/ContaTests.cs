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
    public void Create_ComParametrosValidos_DeveCriarConta()
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

    [Fact]
    public void Create_ComCpfNulo_DeveLancarExcecao()
    {
        // Act
        Action act = () => Conta.Create(null!, "1234", "56789", 1000m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("CPF não pode ser vazio.");
    }

    [Fact]
    public void Create_ComCpfInvalido_DeveLancarExcecao()
    {
        // Act
        Action act = () => Conta.Create("123", "1234", "56789", 1000m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("CPF deve ter exatamente 11 dígitos.");
    }

    [Fact]
    public void Create_ComAgenciaNula_DeveLancarExcecao()
    {
        // Act
        Action act = () => Conta.Create("12345678901", null!, "56789", 1000m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Agência é obrigatória.");
    }

    [Fact]
    public void Create_ComAgenciaInvalida_DeveLancarExcecao()
    {
        // Act
        Action act = () => Conta.Create("12345678901", "12", "56789", 1000m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Agência deve ter exatamente 4 caracteres.");
    }

    [Fact]
    public void Create_ComNumeroContaNulo_DeveLancarExcecao()
    {
        // Act
        Action act = () => Conta.Create("12345678901", "1234", null!, 1000m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Número da conta é obrigatório.");
    }

    [Fact]
    public void Create_ComNumeroContaInvalido_DeveLancarExcecao()
    {
        // Act
        Action act = () => Conta.Create("12345678901", "1234", "ABCDE", 1000m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Número da conta deve conter apenas números.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Create_ComLimiteInvalido_DeveLancarExcecao(decimal limiteInvalido)
    {
        // Act
        Action act = () => Conta.Create("12345678901", "1234", "56789", limiteInvalido);

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
    public void Restore_ComParametrosInvalidos_DeveCriarMesmoAssim()
    {
        // Arrange
        var cpf = Cpf.Restore("abc"); // Restore não valida
        var limite = LimiteDiario.Restore(0.01m, 0m, DateTime.UtcNow.Date);

        // Act
        var conta = Conta.Restore(cpf, "", "XYZ", limite);

        // Assert
        conta.Documento.Numero.Should().Be("abc");
        conta.Agencia.Should().Be("");
        conta.NumeroDaConta.Should().Be("XYZ");
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
    public void RealizarPix_ComValorValido_DeveDebitarLimite()
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
    public void RealizarPix_ComValorAcimaDoLimite_DeveLancarExcecaoESalvarEstado()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();
        var valorAntes = conta.LimitePix.ValorUtilizado;

        // Act
        var act = () => conta.RealizarPix(2000m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Limite diário insuficiente.");
        conta.LimitePix.ValorUtilizado.Should().Be(valorAntes); // invariável mantido
    }

    [Fact]
    public void RealizarPix_ComValorZero_DeveLancarExcecao()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();

        // Act
        var act = () => conta.RealizarPix(0);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Valor deve ser maior ou igual a 0,01.");
    }

    [Fact]
    public void RealizarPix_ComValorNegativo_DeveLancarExcecao()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();

        // Act
        var act = () => conta.RealizarPix(-10);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Valor não pode ser negativo.");
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
    public void AlterarLimite_ComValorValido_DeveAtualizarLimite()
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

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void AlterarLimite_ComValorInvalido_DeveLancarExcecao(decimal limiteInvalido)
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
    public void Delete_ComContaAtiva_DeveMarcarComoDeletada()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();

        // Act
        conta.Delete();

        // Assert
        conta.IsDeleted.Should().BeTrue();
        conta.DeletedAt.Should().NotBeNull();
        conta.DeletedAt!.Value.Date.Should().Be(DateTime.UtcNow.Date);
    }

    [Fact]
    public void Delete_ComContaJaDeletada_DeveLancarExcecao()
    {
        // Arrange
        var conta = _fixture.CriarContaValida();
        conta.Delete();

        // Act
        var act = () => conta.Delete();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("A conta já foi excluída.");
    }
}