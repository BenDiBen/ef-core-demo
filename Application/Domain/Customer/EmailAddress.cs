using System.Text.RegularExpressions;
using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public readonly partial struct EmailAddress
{
    private static readonly Regex EmailRegex = GenerateEmailRegex();

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Validation.Invalid("Email cannot be empty");
            
        return EmailRegex.IsMatch(input) 
            ? Validation.Ok 
            : Validation.Invalid("Invalid email format");
    }

    private static string NormalizeInput(string input) => input.Trim();

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex GenerateEmailRegex();
}
