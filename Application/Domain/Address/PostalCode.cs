using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public partial class PostalCode
{
    private static Validation Validate(string input) =>
        input switch
        {
            _ when !uint.TryParse(input, out _) => Validation.Invalid("Postal code must be numeric"),
            { Length: 4 } => Validation.Ok,
            _ => Validation.Invalid("Postal code must be 4 digits"),
        };

    private static string NormalizeInput(string input) => input.Trim();
}