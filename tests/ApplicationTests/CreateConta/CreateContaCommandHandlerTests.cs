using FluentAssertions;
using FraudSys.App.Features.Commands;
using FraudSys.App.Features.Commands.CreateConta;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Exceptions;
using FraudSys.Domain.Interfaces;
using Moq;

namespace ApplicationTests;

public class CreateContaCommandHandlerTests
{
    private readonly CreateContaCommandHandler _handler;
    private readonly Mock<IContaRepository> _repositoryMock;

    public CreateContaCommandHandlerTests()
    {
        _repositoryMock = new Mock<IContaRepository>();
        _handler = new CreateContaCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ContaNaoExistente_DeveCriarConta()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "1234", "56789", 1000m);
        _repositoryMock.Setup(r => r.ObterContaAsync(command.Documento, command.Agencia, command.NumeroDaConta))
            .ReturnsAsync((Conta?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Documento.Should().Be(command.Documento);
        result.Value.Agencia.Should().Be(command.Agencia);
        result.Value.NumeroDaConta.Should().Be(command.NumeroDaConta);
        result.Value.LimitePix.Should().Be(command.LimitePix);
        _repositoryMock.Verify(r => r.CriarContaAsync(It.IsAny<Conta>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ContaJaExistente_DeveRetornarFailure()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "1234", "56789", 1000m);
        var contaExistente = Conta.Create(command.Documento, command.Agencia, command.NumeroDaConta, command.LimitePix);

        _repositoryMock.Setup(r => r.ObterContaAsync(command.Documento, command.Agencia, command.NumeroDaConta))
            .ReturnsAsync(contaExistente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Já existe uma conta com esses dados.");
        _repositoryMock.Verify(r => r.CriarContaAsync(It.IsAny<Conta>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DomainException_DeveRetornarFailureComMensagem()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "1234", "56789", 0);
        _repositoryMock.Setup(r => r.ObterContaAsync(command.Documento, command.Agencia, command.NumeroDaConta))
            .ReturnsAsync((Conta?)null);
        _repositoryMock.Setup(r => r.CriarContaAsync(It.IsAny<Conta>()))
            .ThrowsAsync(new DomainException("Limite diário deve ser maior que zero."));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Limite diário deve ser maior que zero.");
    }

    [Fact]
    public async Task Handle_ErroInesperado_DeveRetornarFailureGenerica()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "1234", "56789", 1000m);
        _repositoryMock.Setup(r => r.ObterContaAsync(command.Documento, command.Agencia, command.NumeroDaConta))
            .ThrowsAsync(new Exception("Erro de banco"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Erro inesperado ao criar conta.");
    }
}