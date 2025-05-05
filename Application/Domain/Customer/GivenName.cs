using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public partial class GivenName
{
    private static Validation Validate(string input) =>
        input switch
        {
            { Length: < 2 } => Validation.Invalid("Given name must be at least 2 characters"),
            { Length: > 50 } => Validation.Invalid("Given name must let than 50 characters"),
            _ when string.IsNullOrWhiteSpace(input) => Validation.Invalid("Given name cannot be white space"),
            _ when input.Contains(' ') => Validation.Invalid("Given name cannot contain any spaces"),
            _ => Validation.Ok
        };

    private static string NormalizeInput(string input) => input.Trim();
}