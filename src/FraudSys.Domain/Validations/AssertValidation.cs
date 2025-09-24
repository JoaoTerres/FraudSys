using System.Text.RegularExpressions;

namespace FraudSys.Domain.Validations;

public static class AssertValidation
{
    public static void ValidateIfNull(object? obj, string message)
    {
        if (obj is null)
            throw new ArgumentException(message);
    }

    public static void ValidateIfNullOrEmpty(string? text, string message)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException(message);
    }

    public static void ValidateLength(string text, int minLength, int maxLength, string message)
    {
        var length = text.Trim().Length;
        if (length < minLength || length > maxLength)
            throw new ArgumentException(message);
    }

    public static void ValidateIfFalse(bool condition, string message)
    {
        if (!condition)
            throw new ArgumentException(message);
    }

    public static void ValidateIfLowerThen(long value, long min, string message)
    {
        if (value < min)
            throw new ArgumentException(message);
    }
    public static void ValidateCpfFormat(string text, string message)
    {
        if (!Regex.IsMatch(text, @"^[0-9]+$"))
            throw new ArgumentException(message);
    }
}