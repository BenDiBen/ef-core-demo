using System.Text.RegularExpressions;
using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public readonly partial struct PhoneNumber
{
    private static readonly Regex PhoneNumberRegex = GeneratePhoneNumberRegex();
    private static readonly Regex InputRegex = GenerateInputRegex();

    private static string NormalizeInput(string input)
    {
        var inputComparison = InputRegex.Match(input);

        return !inputComparison.Success
            ? input
            : $"+27 {inputComparison.Groups[1].Value} {inputComparison.Groups[2].Value} {inputComparison.Groups[3].Value}";
    }

    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Validation.Invalid("Phone number cannot be empty");

        return PhoneNumberRegex.IsMatch(input)
            ? Validation.Ok
            : Validation.Invalid("Phone number must be in format '+27 00 000 0000'");
    }

    [GeneratedRegex(@"^(?:0|\+27)(?:\s|-)(\d{2})(?:\s|-)(\d{3})(?:\s|-)(\d{4})$")]
    private static partial Regex GenerateInputRegex();
    
    [GeneratedRegex(@"^\+27\s{0,1}\d{2}\s{0,1}\d{3}\s{0,1}\d{4}$")]
    private static partial Regex GeneratePhoneNumberRegex();
}