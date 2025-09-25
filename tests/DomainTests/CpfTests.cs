using FluentAssertions;
using FraudSys.Domain.Exceptions;
using FraudSys.Domain.ValueObjects;

namespace DomainTests;

public class CpfTests
{
    [Fact]
    public void Create_CpfValido_DeveRetornarInstancia()
    {
        // Arrange
        var numero = "12345678901";

        // Act
        var cpf = Cpf.Create(numero);

        // Assert
        cpf.Should().NotBeNull();
        cpf.Numero.Should().Be(numero);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_CpfNuloOuVazio_DeveLancarExcecao(string? documento)
    {
        // Act
        Action act = () => Cpf.Create(documento!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("CPF não pode ser vazio.");
    }

    [Theory]
    [InlineData("123")]        
    [InlineData("1234567890")]   
    [InlineData("123456789012")]
    public void Create_CpfComTamanhoInvalido_DeveLancarExcecao(string documento)
    {
        // Act
        Action act = () => Cpf.Create(documento);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("CPF deve ter exatamente 11 dígitos.");
    }

    [Theory]
    [InlineData("1234567890A")] 
    [InlineData("12345-67890")] 
    [InlineData("12345.67890")] 
    public void Create_CpfComCaracteresInvalidos_DeveLancarExcecao(string documento)
    {
        // Act
        Action act = () => Cpf.Create(documento);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("CPF deve conter apenas números");
    }

    [Fact]
    public void Restore_DeveCriarCpfSemValidacao()
    {
        // Arrange
        var numero = "abc";

        // Act
        var cpf = Cpf.Restore(numero);

        // Assert
        cpf.Should().NotBeNull();
        cpf.Numero.Should().Be(numero);
    }

    [Fact]
    public void Equals_CpfsIguais_DeveRetornarTrue()
    {
        // Arrange
        var cpf1 = Cpf.Create("12345678901");
        var cpf2 = Cpf.Create("12345678901");

        // Act
        var iguais = cpf1.Equals(cpf2);

        // Assert
        iguais.Should().BeTrue();
        cpf1.Should().Be(cpf2);
    }

    [Fact]
    public void Equals_CpfsDiferentes_DeveRetornarFalse()
    {
        // Arrange
        var cpf1 = Cpf.Create("12345678901");
        var cpf2 = Cpf.Create("10987654321");

        // Act
        var iguais = cpf1.Equals(cpf2);

        // Assert
        iguais.Should().BeFalse();
        cpf1.Should().NotBe(cpf2);
    }

    [Fact]
    public void GetHashCode_CpfsIguais_DeveRetornarMesmoHash()
    {
        // Arrange
        var cpf1 = Cpf.Create("12345678901");
        var cpf2 = Cpf.Create("12345678901");

        // Act
        var hash1 = cpf1.GetHashCode();
        var hash2 = cpf2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_DeveRetornarNumeroDoCpf()
    {
        // Arrange
        var numero = "12345678901";
        var cpf = Cpf.Create(numero);

        // Act
        var resultado = cpf.ToString();

        // Assert
        resultado.Should().Be(numero);
    }
}