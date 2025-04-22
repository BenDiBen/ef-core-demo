using Vogen;

namespace EfCoreDemo.Domain;

[ValueObject<Guid>]
public readonly partial struct CustomerId
{
    public static CustomerId New() => From(Guid.NewGuid());
}