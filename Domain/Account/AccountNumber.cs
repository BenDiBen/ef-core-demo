using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public partial class AccountNumber
{
    private const int ExpectedLength = 10;
    private static Validation Validate(string input) =>
        input switch
        {
            { Length: ExpectedLength } => Validation.Ok,
            _ when !int.TryParse(input, out _) => Validation.Invalid("Account number must only contain numeric characters"), 
            _ => Validation.Invalid($"Account number must be {ExpectedLength} characters")
        };

    private static string NormalizeInput(string input) => input.Trim();
}