using FluentAssertions;
using FraudSys.Domain.Exceptions;
using FraudSys.Domain.ValueObjects;

namespace DomainTests;

public class LimiteDiarioTests
{
    [Fact]
    public void Create_ComValorValido_DeveCriarLimite()
    {
        // Arrange
        var valor = 1000m;

        // Act
        var limite = LimiteDiario.Create(valor);

        // Assert
        limite.Should().NotBeNull();
        limite.ValorMaximo.Should().Be(valor);
        limite.ValorUtilizado.Should().Be(0);
        limite.DataReferencia.Should().Be(DateTime.UtcNow.Date);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Create_ComValorInvalido_DeveLancarExcecao(decimal valorInvalido)
    {
        // Act
        Action act = () => LimiteDiario.Create(valorInvalido);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Limite diário deve ser maior que zero.");
    }

    [Fact]
    public void Restore_DeveRecriarEstadoDoObjeto()
    {
        // Arrange
        var max = 500m;
        var usado = 200m;
        var referencia = DateTime.UtcNow.Date.AddDays(-1);

        // Act
        var limite = LimiteDiario.Restore(max, usado, referencia);

        // Assert
        limite.ValorMaximo.Should().Be(max);
        limite.ValorUtilizado.Should().Be(usado);
        limite.DataReferencia.Should().Be(referencia);
    }

    [Fact]
    public void Disponivel_ComValorDentroDoLimite_DeveRetornarTrue()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);

        // Act
        var disponivel = limite.Disponivel(200m);

        // Assert
        disponivel.Should().BeTrue();
    }

    [Fact]
    public void Disponivel_ComValorAcimaDoLimite_DeveRetornarFalse()
    {
        // Arrange
        var limite = LimiteDiario.Create(100m);

        // Act
        var disponivel = limite.Disponivel(200m);

        // Assert
        disponivel.Should().BeFalse();
    }

    [Fact]
    public void Disponivel_EmNovoDia_DeveResetarValorUtilizado()
    {
        // Arrange
        var limite = LimiteDiario.Restore(500m, 300m, DateTime.UtcNow.Date.AddDays(-1));

        // Act
        var disponivel = limite.Disponivel(200m);

        // Assert
        disponivel.Should().BeTrue();
        limite.ValorUtilizado.Should().Be(0);
        limite.DataReferencia.Should().Be(DateTime.UtcNow.Date);
    }

    [Fact]
    public void Disponivel_ComValorIgualAoLimiteRestante_DeveRetornarTrue()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);
        limite.Debitar(200m);

        // Act
        var disponivel = limite.Disponivel(300m);

        // Assert
        disponivel.Should().BeTrue();
    }

    [Fact]
    public void Debitar_ComValorValido_DeveAtualizarValorUtilizado()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);

        // Act
        limite.Debitar(100m);

        // Assert
        limite.ValorUtilizado.Should().Be(100m);
    }

    [Fact]
    public void Debitar_ComValorNegativo_DeveLancarExcecao()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);

        // Act
        var act = () => limite.Debitar(-10m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Valor não pode ser negativo.");
    }

    [Fact]
    public void Debitar_ComValorZero_DeveLancarExcecao()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);

        // Act
        var act = () => limite.Debitar(0m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Valor deve ser maior ou igual a 0,01.");
    }

    [Fact]
    public void Debitar_ComValorMaiorQueDisponivel_DeveLancarExcecao()
    {
        // Arrange
        var limite = LimiteDiario.Create(100m);

        // Act
        var act = () => limite.Debitar(200m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Limite diário insuficiente.");
    }

    [Fact]
    public void Debitar_ComValorComMuitasCasasDecimais_DeveArredondarParaDuas()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);

        // Act
        limite.Debitar(100.999m);

        // Assert
        limite.ValorUtilizado.Should().Be(100.99m);
    }

    [Fact]
    public void Debitar_ComValorIgualAoLimiteRestante_DevePermitir()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);
        limite.Debitar(200m);

        // Act
        limite.Debitar(300m);

        // Assert
        limite.ValorUtilizado.Should().Be(500m);
        limite.Disponivel(1m).Should().BeFalse();
    }

    [Fact]
    public void Debitar_ComMultiplosValores_DeveAcumularCorretamente()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);

        // Act
        limite.Debitar(100m);
        limite.Debitar(150m);

        // Assert
        limite.ValorUtilizado.Should().Be(250m);
        limite.Disponivel(250m).Should().BeTrue();
    }

    [Fact]
    public void Debitar_AposResetEmNovoDia_DevePermitirValorMaximoNovamente()
    {
        // Arrange
        var limite = LimiteDiario.Restore(300m, 300m, DateTime.UtcNow.Date.AddDays(-1));

        // Act
        limite.Debitar(300m);

        // Assert
        limite.ValorUtilizado.Should().Be(300m);
        limite.Disponivel(1m).Should().BeFalse();
    }

    [Fact]
    public void Debitar_ComValorMinimoPermitido_DevePermitir()
    {
        // Arrange
        var limite = LimiteDiario.Create(100m);

        // Act
        limite.Debitar(0.01m);

        // Assert
        limite.ValorUtilizado.Should().Be(0.01m);
    }

    [Fact]
    public void Debitar_ComValorArredondadoProximoAoLimite_DevePermitir()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);

        // Act
        limite.Debitar(499.9999m);

        // Assert
        limite.ValorUtilizado.Should().Be(499.99m);
        limite.Disponivel(0.01m).Should().BeTrue();
    }
}