namespace FraudSys.App.Exceptions;

public class ApplicationException : Exception
{
    public ApplicationException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}

public class NotFoundException : ApplicationException
{
    public NotFoundException(string documento, string agencia, string numero)
        : base($"Conta não encontrada: Documento={documento}, Agência={agencia}, Conta={numero}")
    {
    }
}