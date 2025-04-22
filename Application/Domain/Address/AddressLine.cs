using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public partial class AddressLine
{
    private const int MinLength = 5;
    private const int MaxLength = 200;
    private static Validation Validate(string input) =>
        input switch
        {
            _ when input.Length < MinLength => Validation.Invalid("Address line must be at least 5 characters"),
            _ when input.Length > MaxLength => Validation.Invalid("Address line must be less than 200 characters"),
            _ => Validation.Ok,
        };

    private static string NormalizeInput(string input) => input.Trim();
}