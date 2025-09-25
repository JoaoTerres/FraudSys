using FluentValidation.TestHelper;
using FraudSys.App.Features.Commands;
using FraudSys.App.Features.Commands.CreateConta;

namespace ApplicationTests;

public class CreateContaCommandValidatorTests
{
    private readonly CreateContaCommandValidator _validator = new();
    
    [Fact]
    public void Documento_Nulo_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand(null!, "1234", "56789", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Documento)
            .WithErrorMessage("Documento é obrigatório");
    }

    [Fact]
    public void Documento_Vazio_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("", "1234", "56789", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Documento)
            .WithErrorMessage("Documento (CPF) é obrigatório");
    }

    [Fact]
    public void Documento_ComMenosDe11Digitos_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("123", "1234", "56789", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Documento)
            .WithErrorMessage("Documento deve ter exatamente 11 dígitos");
    }

    [Fact]
    public void Documento_ComMaisDe11Digitos_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("123456789012", "1234", "56789", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Documento)
            .WithErrorMessage("Documento deve ter exatamente 11 dígitos");
    }

    [Fact]
    public void Documento_ComLetras_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("12345A78901", "1234", "56789", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Documento)
            .WithErrorMessage("Documento deve conter apenas números");
    }
    
    [Fact]
    public void Agencia_Vazia_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "", "56789", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Agencia)
            .WithErrorMessage("Agência é obrigatória");
    }

    [Fact]
    public void Agencia_ComMenosDe4Digitos_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "12", "56789", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Agencia)
            .WithErrorMessage("Agência deve ter exatamente 4 dígitos");
    }

    [Fact]
    public void Agencia_ComMaisDe4Digitos_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "12345", "56789", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Agencia)
            .WithErrorMessage("Agência deve ter exatamente 4 dígitos");
    }

    [Fact]
    public void Agencia_ComLetras_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "12A4", "56789", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Agencia)
            .WithErrorMessage("Agência deve conter apenas números");
    }
    
    [Fact]
    public void NumeroConta_Vazio_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "1234", "", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumeroDaConta)
            .WithErrorMessage("Número da conta é obrigatório");
    }

    [Fact]
    public void NumeroConta_ComMaisDe10Caracteres_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "1234", "12345678901", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumeroDaConta)
            .WithErrorMessage("Número da conta deve ter no máximo 10 caracteres");
    }

    [Fact]
    public void NumeroConta_ComLetras_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "1234", "12AB3", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumeroDaConta)
            .WithErrorMessage("Número da conta deve conter apenas números");
    }
    
    [Fact]
    public void LimitePix_MenorOuIgualAZero_DeveFalhar()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "1234", "56789", 0);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LimitePix)
            .WithErrorMessage("O limite PIX deve ser maior que zero");
    }
    
    [Fact]
    public void ComandoValido_DevePassar()
    {
        // Arrange
        var command = new CreateContaCommand("12345678901", "1234", "56789", 1000m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}