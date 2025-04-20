using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<string>]
public partial class Gender
{
    public static readonly Gender Male = From("Male");
    public static readonly Gender Female = From("Female");
    public static readonly Gender Other = From("Other");
    public static readonly Gender Unknown = From("Unknown");
}
