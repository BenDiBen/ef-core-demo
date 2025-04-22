using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public partial class AccountType
{
    public static readonly AccountType Cheque = From("Cheque");
    public static readonly AccountType Savings = From("Savings");
    public static readonly AccountType Transaction = From("Transaction");
    public static readonly AccountType Default = Cheque;

    private static string NormalizeInput(string input) => input.Trim();
}