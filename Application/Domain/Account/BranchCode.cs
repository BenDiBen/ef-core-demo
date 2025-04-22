using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public partial class BranchCode
{
    public static readonly BranchCode Pretoria = new("410");
    public static readonly BranchCode Johannesburg = new("420");
    public static readonly BranchCode Durban = new("430");
    public static readonly BranchCode Capetown = new("440");
    
    private static Validation Validate(string input) =>
        input switch
        {
            _ when input.Length != 3 => Validation.Invalid("Branch code must be 3 characters"),
            _ when !int.TryParse(input, out _) => Validation.Invalid("Branch code must be numeric"),
            _ => Validation.Ok
        };
}