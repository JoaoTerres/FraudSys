using FluentAssertions;
using FraudSys.Domain.Exceptions;
using FraudSys.Domain.ValueObjects;

namespace DomainTests;

public class LimiteDiarioTests
{
    [Fact]
    public void Create_ValorValido_DeveCriarLimite()
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
    public void Create_ValorInvalido_DeveLancarExcecao(decimal valorInvalido)
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
    public void Disponivel_ValorDentroDoLimite_DeveRetornarTrue()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);
        var valor = 200m;

        // Act
        var disponivel = limite.Disponivel(valor);

        // Assert
        disponivel.Should().BeTrue();
    }

    [Fact]
    public void Disponivel_ValorAcimaDoLimite_DeveRetornarFalse()
    {
        // Arrange
        var limite = LimiteDiario.Create(100m);
        var valor = 200m;

        // Act
        var disponivel = limite.Disponivel(valor);

        // Assert
        disponivel.Should().BeFalse();
    }

    [Fact]
    public void Debitar_ValorValido_DeveAtualizarValorUtilizado()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);
        var valor = 100m;

        // Act
        limite.Debitar(valor);

        // Assert
        limite.ValorUtilizado.Should().Be(valor);
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(0)]
    public void Debitar_ValorInvalido_DeveLancarExcecao(decimal valorInvalido)
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);

        // Act
        Action act = () => limite.Debitar(valorInvalido);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Valor*");
    }

    [Fact]
    public void Debitar_ValorMaiorQueDisponivel_DeveLancarExcecao()
    {
        // Arrange
        var limite = LimiteDiario.Create(100m);
        var valor = 200m;

        // Act
        Action act = () => limite.Debitar(valor);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Limite diário insuficiente.");
    }

    [Fact]
    public void Disponivel_EmNovoDia_DeveResetarValorUtilizado()
    {
        // Arrange
        var limite = LimiteDiario.Restore(500m, 300m, DateTime.UtcNow.Date.AddDays(-1));
        var valor = 200m;

        // Act
        var disponivel = limite.Disponivel(valor);

        // Assert
        disponivel.Should().BeTrue();
        limite.ValorUtilizado.Should().Be(0); 
        limite.DataReferencia.Should().Be(DateTime.UtcNow.Date);
    }

    [Fact]
    public void Debitar_ValorComMuitasCasasDecimais_DeveArredondarParaDuas()
    {
        // Arrange
        var limite = LimiteDiario.Create(500m);
        var valor = 100.999m;

        // Act
        limite.Debitar(valor);

        // Assert
        limite.ValorUtilizado.Should().Be(100.99m); 
    }
    
    [Fact]
    public void Disponivel_ValorIgualAoLimiteRestante_DeveRetornarTrue()
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
    public void Debitar_ValorIgualAoLimiteRestante_DevePermitir()
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
    public void Debitar_MultiplasVezes_DeveAcumularCorretamente()
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
    public void Debitar_AposResetDeNovoDia_DevePermitirValorMaximoNovamente()
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
    public void Debitar_ValorIgualAMinimoPermitido_DevePermitir()
    {
        // Arrange
        var limite = LimiteDiario.Create(100m);

        // Act
        limite.Debitar(0.01m);

        // Assert
        limite.ValorUtilizado.Should().Be(0.01m);
    }
    
    [Fact]
    public void Debitar_ValorArredondadoProximoAoLimite_DevePermitir()
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